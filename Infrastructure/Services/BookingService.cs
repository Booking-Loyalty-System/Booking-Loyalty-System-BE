using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;

    public BookingService(IApplicationDbContext context)
    {
        _context = context;
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

        // Validate booking date not in the past
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (request.BookingDate < today)
            throw new AppException("Booking date cannot be in the past.", 400);

        // Tier-based booking window
        var maxDays = customer.Tier.BookingWindow;
        var maxDate = today.AddDays(maxDays);
        if (request.BookingDate > maxDate)
            throw new AppException($"Your {customer.Tier.TierName} tier allows booking up to {maxDays} days in advance.", 400);

        // Auto-assign an available wash bay
        var washBay = await _context.WashBays
            .Include(wb => wb.Branch)
            .FirstOrDefaultAsync(wb => wb.Status == WashBayStatus.Available)
            ?? throw new AppException("No available wash bay at the moment.", 400);

        // Generate booking code (6 chars A-Z0-9)
        var bookingCode = await GenerateUniqueBookingCodeAsync();

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            BookingCode = bookingCode,
            CustomerId = customer.Id,
            VehicleId = vehicle.Id,
            WashPackageId = washPackage.Id,
            BayId = washBay.Id,
            BranchId = washBay.BranchId,
            BookingDate = request.BookingDate,
            StartTime = request.StartTime,
            TotalPrice = washPackage.Price,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return MapToResponse(booking, washPackage, vehicle);
    }

    public async Task<BookingResponse> GetBookingByIdAsync(Guid userId, Guid bookingId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle);
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
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => MapToResponse(b, b.WashPackage, b.Vehicle)).ToList();
    }

    public async Task<BookingResponse> CancelBookingAsync(Guid userId, Guid bookingId, string? reason)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be cancelled.", 400);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(booking, booking.WashPackage, booking.Vehicle);
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

    private static BookingResponse MapToResponse(Booking booking, WashPackage washPackage, Vehicle vehicle)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = washPackage.Name,
            DurationMinutes = washPackage.DurationMinutes,
            BookingDate = booking.BookingDate,
            StartTime = booking.StartTime,
            VehiclePlate = vehicle.LicensePlate,
            VehicleName = vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt
        };
    }
}
