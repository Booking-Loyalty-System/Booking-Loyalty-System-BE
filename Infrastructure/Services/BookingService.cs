using Application.Common;
using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly ILoyaltyService _loyaltyService;
    private readonly BookingOptions _options;
    private readonly TimeZoneInfo _shopTimeZone;

    public BookingService(
        IApplicationDbContext context,
        ILoyaltyService loyaltyService,
        IOptions<BookingOptions> options)
    {
        _context = context;
        _loyaltyService = loyaltyService;
        _options = options.Value;
        _shopTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZoneId);
    }

    public async Task<BookingResponse> CreateBookingAsync(Guid userId, CreateBookingRequest request)
    {
        var customer = await _context.Customers
                           .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == request.VehicleId && v.CustomerId == customer.Id && !v.IsDeleted)
            ?? throw new AppException("Vehicle not found or does not belong to you.", 404);

        var washPackage = await _context.WashPackages
            .FirstOrDefaultAsync(wp => wp.Id == request.WashPackageId && wp.IsActive)
            ?? throw new AppException("Wash package not found or inactive.", 404);

        // The customer must pick a branch first; only an open branch can take bookings.
        var branch = await _context.Branches
            .FirstOrDefaultAsync(b => b.Id == request.BranchId)
            ?? throw new AppException("Branch not found.", 404);

        if (branch.Status != BranchStatus.Active)
            throw new AppException("The selected branch is not currently open for booking.", 400);

        // Shop-local "now" — bookings are expressed in the shop's local clock, not UTC.
        var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _shopTimeZone);
        var today = DateOnly.FromDateTime(nowLocal);
        var nowTime = TimeOnly.FromDateTime(nowLocal);

        if (request.BookingDate < today)
            throw new AppException("Booking date cannot be in the past.", 400);

        // For same-day bookings, the start time must still be in the future
        if (request.BookingDate == today && request.StartTime <= nowTime)
            throw new AppException("Start time cannot be in the past.", 400);

        // Tier-based booking window
        var maxDays = customer.Tier.BookingWindow;
        var maxDate = today.AddDays(maxDays);
        if (request.BookingDate > maxDate)
            throw new AppException($"Your {customer.Tier} tier allows booking up to {maxDays} days in advance.", 400);

        // The booking occupies the bay for [StartTime, StartTime + package duration)
        var endTime = request.StartTime.Add(TimeSpan.FromMinutes(washPackage.DurationMinutes));
        if (endTime <= request.StartTime)
            throw new AppException("The selected start time and wash duration are invalid (the wash would run past midnight).", 400);

        // The whole wash must fit inside the shop's operating hours.
        if (request.StartTime < _options.OpenTime || endTime > _options.CloseTime)
            throw new AppException(
                $"Booking time must be within business hours " +
                $"({_options.OpenTime:HH\\:mm}–{_options.CloseTime:HH\\:mm}).", 400);

        // A single vehicle cannot be in two places at once — block overlapping active bookings.
        var vehicleConflict = await _context.Bookings.AnyAsync(b =>
            b.VehicleId == vehicle.Id &&
            b.TimeSlot != null &&
            b.TimeSlot.Date == request.BookingDate &&
            (b.Status == BookingStatus.Pending ||
             b.Status == BookingStatus.Confirmed ||
             b.Status == BookingStatus.InProgress) &&
            b.TimeSlot.StartTime < endTime && b.TimeSlot.EndTime > request.StartTime);

        if (vehicleConflict)
            throw new AppException(
                "This vehicle already has a booking that overlaps the selected time.", 409);

        // Reserve a wash bay + time slot atomically so the same bay can never be
        // double-booked for an overlapping window (Serializable transaction).
        await using var transaction = await _context.BeginTransactionAsync();

        var bay = await FindAvailableBayAsync(branch.Id, vehicle.Type, request.BookingDate, request.StartTime, endTime)
            ?? throw new AppException(
                "No wash bay is available at this branch for the selected date and time. Please choose another slot.", 409);

        // Generate booking code (6 chars A-Z0-9)
        var bookingCode = await GenerateUniqueBookingCodeAsync();

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            BookingCode = bookingCode,
            CustomerId = customer.Id,
            VehicleId = vehicle.Id,
            WashPackageId = washPackage.Id,
            BranchId = branch.Id,
            BayId = bay.Id,
            BookingDate = request.BookingDate,
            StartTime = request.StartTime,
            TotalPrice = washPackage.Price,
            Status = BookingStatus.Confirmed,
            CreatedAt = DateTime.UtcNow
        };

        var slot = new TimeSlot
        {
            Id = Guid.NewGuid(),
            WashBayId = bay.Id,
            Date = request.BookingDate,
            StartTime = request.StartTime,
            EndTime = endTime,
            Status = TimeSlotStatus.Booked,
            BookingId = booking.Id
        };
        booking.TimeSlotId = slot.Id;

        _context.Bookings.Add(booking);
        _context.TimeSlots.Add(slot);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return MapToResponse(booking, washPackage, vehicle, slot, bay);
    }

    /// <summary>
    /// Finds the first wash bay that is operational, supports the vehicle type, and
    /// has no active time slot overlapping the requested [start, end) window.
    /// Returns null when every bay is busy or none supports the vehicle type.
    /// </summary>
    private async Task<WashBay?> FindAvailableBayAsync(
        Guid branchId, VehicleType vehicleType, DateOnly date, TimeOnly start, TimeOnly end)
    {
        var typeName = vehicleType.ToString();

        var bays = await _context.WashBays
            .Where(b => b.BranchId == branchId && b.Status == WashBayStatus.Available)
            .OrderBy(b => b.Name)
            .ToListAsync();

        foreach (var bay in bays)
        {
            // A bay with no declared SupportedTypes is treated as supporting all types.
            var supported = bay.SupportedTypes
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (supported.Length > 0 &&
                !supported.Any(s => string.Equals(s, typeName, StringComparison.OrdinalIgnoreCase)))
                continue;

            var hasConflict = await _context.TimeSlots.AnyAsync(ts =>
                ts.WashBayId == bay.Id &&
                ts.Date == date &&
                (ts.Status == TimeSlotStatus.Booked || ts.Status == TimeSlotStatus.InProgress) &&
                ts.StartTime < end && ts.EndTime > start);

            if (!hasConflict)
                return bay;
        }

        return null;
    }

    public async Task<BookingResponse> GetBookingByIdAsync(Guid userId, Guid bookingId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.TimeSlot)
                .ThenInclude(ts => ts!.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.TimeSlot);
    }

    public async Task<List<BookingResponse>> GetMyBookingsAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var bookings = await _context.Bookings
            .Where(b => b.CustomerId == customer.Id)
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.TimeSlot)
                .ThenInclude(ts => ts!.WashBay)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => MapToResponse(b, b.WashPackage, b.Vehicle, b.TimeSlot)).ToList();
    }

    public async Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.TimeSlot)
                .ThenInclude(ts => ts!.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be cancelled.", 400);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        // Release the reserved bay/time slot so the window becomes bookable again.
        var slot = booking.TimeSlot;
        if (slot != null)
        {
            _context.TimeSlots.Remove(slot);
            booking.TimeSlotId = null;
        }

        await _context.SaveChangesAsync();

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, slot);
    }

    public async Task<BookingResponse> CompleteBookingAsync(Guid bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.TimeSlot)
                .ThenInclude(ts => ts!.WashBay)
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status is BookingStatus.Pending or BookingStatus.Cancelled)
            throw new AppException("Only confirmed or in-progress bookings can be completed.", 400);

        // Confirmed/InProgress -> Completed. An already-Completed booking falls through
        // so a prior partial award (status saved but points failed) can be retried.
        if (booking.Status != BookingStatus.Completed)
        {
            booking.Status = BookingStatus.Completed;
            booking.UpdatedAt = DateTime.UtcNow;
            if (booking.TimeSlot != null)
                booking.TimeSlot.Status = TimeSlotStatus.Completed;
            await _context.SaveChangesAsync();
        }

        // Award loyalty points — idempotent, so a repeated call for the same booking is a no-op.
        await _loyaltyService.AwardPointsForBookingAsync(bookingId);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle, booking.TimeSlot);
    }

    private async Task<string> GenerateUniqueBookingCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string code;

        do
        {
            code = new string(Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        } while (await _context.Bookings.AnyAsync(b => b.BookingCode == code));

        return code;
    }

    private static BookingResponse MapToResponse(
        Booking booking, WashPackage washPackage, Vehicle vehicle, TimeSlot? slot, WashBay? bay = null)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = washPackage.Name,
            DurationMinutes = washPackage.DurationMinutes,
            BookingDate = booking.BookingDate,
            StartTime = booking.StartTime,
            EndTime = slot?.EndTime,
            WashBayName = bay?.Name ?? slot?.WashBay?.Name,
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt
        };
    }
}
