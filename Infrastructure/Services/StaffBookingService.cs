using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StaffBookingService : IStaffBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly ILoyaltyService _loyaltyService;

    public StaffBookingService(IApplicationDbContext context, ILoyaltyService loyaltyService)
    {
        _context = context;
        _loyaltyService = loyaltyService;
    }

    public async Task<List<BookingResponseData>> GetByDateAsync(DateOnly date)
    {
        var bookings = await _context.Bookings
            .Where(b => b.BookingDate == date)
            .Include(b => b.Vehicle)
            .Include(b => b.WashPackage)
            .OrderBy(b => b.StartTime)
            .ToListAsync();

        var ids = bookings.Select(b => b.Id).ToList();

        // Batch-load awarded points for the day's bookings to avoid an N+1 query.
        var pointsByBooking = await _context.LoyaltyTransactions
            .Where(lt => lt.Type == LoyaltyTransactionType.Earn
                         && lt.BookingId != null
                         && ids.Contains(lt.BookingId.Value))
            .ToDictionaryAsync(lt => lt.BookingId!.Value, lt => lt.Points);

        return bookings
            .Select(b => MapToResponseData(
                b,
                pointsByBooking.TryGetValue(b.Id, out var points) ? points : (int?)null))
            .ToList();
    }

    public async Task<BookingResponseData> CheckInAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "check-in", BookingStatus.Confirmed);

        booking.Status = BookingStatus.CheckedIn;
        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await BuildResponseAsync(booking);
    }

    public async Task<BookingResponseData> QueueAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "queue", BookingStatus.CheckedIn);

        booking.Status = BookingStatus.Queued;
        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await BuildResponseAsync(booking);
    }

    public async Task<BookingResponseData> StartAsync(Guid bookingId, Guid staffId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "start", BookingStatus.Queued);

        if (staffId == Guid.Empty)
            throw new AppException("staffId is required to start a service.", 400);

        var isValidStaff = await _context.Users
            .AnyAsync(u => u.Id == staffId && u.Role == UserRole.Staff && u.IsActive);
        if (!isValidStaff)
            throw new AppException("Assigned staff not found or is not an active staff member.", 400);

        booking.AssignedStaffId = staffId;
        booking.Status = BookingStatus.InProgress;
        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await BuildResponseAsync(booking);
    }

    public async Task<BookingResponseData> FinishAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "finish", BookingStatus.InProgress);

        booking.Status = BookingStatus.Completed;
        booking.UpdatedAt = DateTime.UtcNow;
        if (booking.TimeSlot != null)
            booking.TimeSlot.Status = TimeSlotStatus.Completed;
        await _context.SaveChangesAsync();

        return await BuildResponseAsync(booking);
    }

    public async Task<BookingResponseData> CheckoutAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "checkout", BookingStatus.Completed);

        booking.Status = BookingStatus.CheckedOut;
        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Award loyalty points on checkout (idempotent — one Earn row per booking).
        await _loyaltyService.AwardPointsForBookingAsync(bookingId);

        return await BuildResponseAsync(booking);
    }

    public async Task<BookingResponseData> CancelAsync(Guid bookingId, string? reason)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "cancel",
            BookingStatus.Pending, BookingStatus.Confirmed,
            BookingStatus.CheckedIn, BookingStatus.Queued);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;
        ReleaseTimeSlot(booking);
        await _context.SaveChangesAsync();

        return await BuildResponseAsync(booking);
    }

    public async Task<BookingResponseData> NoShowAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "no-show",
            BookingStatus.Confirmed, BookingStatus.CheckedIn);

        booking.Status = BookingStatus.NoShow;
        booking.UpdatedAt = DateTime.UtcNow;
        ReleaseTimeSlot(booking);
        await _context.SaveChangesAsync();

        return await BuildResponseAsync(booking);
    }

    private async Task<Booking> LoadAsync(Guid bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Vehicle)
            .Include(b => b.WashPackage)
            .Include(b => b.TimeSlot)
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);
    }

    /// <summary>Releases the reserved bay/time slot so the window becomes bookable again.</summary>
    private void ReleaseTimeSlot(Booking booking)
    {
        if (booking.TimeSlot != null)
        {
            _context.TimeSlots.Remove(booking.TimeSlot);
            booking.TimeSlotId = null;
        }
    }

    private static void EnsureStatus(Booking booking, string action, params BookingStatus[] allowed)
    {
        if (!allowed.Contains(booking.Status))
            throw new AppException(
                $"Cannot {action} a booking in '{booking.Status}' status. " +
                $"Allowed from: {string.Join(", ", allowed)}.", 400);
    }

    private async Task<BookingResponseData> BuildResponseAsync(Booking booking)
    {
        var points = await _context.LoyaltyTransactions
            .Where(lt => lt.BookingId == booking.Id && lt.Type == LoyaltyTransactionType.Earn)
            .Select(lt => (int?)lt.Points)
            .FirstOrDefaultAsync();

        return MapToResponseData(booking, points);
    }

    private static BookingResponseData MapToResponseData(Booking booking, int? pointsEarned)
    {
        return new BookingResponseData
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            VehicleId = booking.VehicleId,
            VehicleName = booking.Vehicle?.VehicleName,
            LicensePlate = booking.Vehicle?.LicensePlate,
            WashPackageId = booking.WashPackageId,
            ServiceName = booking.WashPackage?.Name,
            BranchId = booking.BranchId,
            BookingDate = booking.BookingDate.ToString("yyyy-MM-dd"),
            StartTime = booking.StartTime.ToString("HH\\:mm"),
            Status = booking.Status.ToString(),
            TotalAmount = booking.TotalPrice,
            PointsEarned = pointsEarned,
            CreatedAt = booking.CreatedAt.ToString("o")
        };
    }
}
