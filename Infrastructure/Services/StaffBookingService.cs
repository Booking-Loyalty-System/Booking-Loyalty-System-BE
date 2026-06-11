using Application.DTOs.Staff;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StaffBookingService : IStaffBookingService
{
    private readonly IApplicationDbContext _context;

    public StaffBookingService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StaffBookingResponse>> GetTodayBookingsAsync(Guid branchId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var query = _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.Customer)
            .Include(b => b.WashBay)
            .Include(b => b.Branch)
            .Where(b => b.BookingDate == today);

        if (branchId != Guid.Empty)
            query = query.Where(b => b.BranchId == branchId);

        var bookings = await query
            .OrderBy(b => b.StartTime)
            .ToListAsync();

        return bookings.Select(MapToResponse).ToList();
    }

    public async Task<StaffBookingResponse> SearchByBookingCodeAsync(string bookingCode)
    {
        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.Customer)
            .Include(b => b.WashBay)
            .Include(b => b.Branch)
            .FirstOrDefaultAsync(b => b.BookingCode.ToUpper() == bookingCode.ToUpper())
            ?? throw new AppException("Booking not found.", 404);

        return MapToResponse(booking);
    }

    public async Task<StaffBookingResponse> ConfirmBookingAsync(Guid bookingId)
    {
        var booking = await GetBookingWithIncludes(bookingId);

        if (booking.Status != BookingStatus.Pending)
            throw new AppException("Only Pending bookings can be confirmed.", 400);

        booking.Status = BookingStatus.Confirmed;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(booking);
    }

    public async Task<StaffBookingResponse> StartWashAsync(Guid bookingId)
    {
        var booking = await GetBookingWithIncludes(bookingId);

        if (booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Confirmed bookings can be started.", 400);

        booking.Status = BookingStatus.InProgress;
        booking.WashBay.Status = WashBayStatus.InProgress;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(booking);
    }

    public async Task<StaffBookingResponse> CompleteWashAsync(Guid bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.Customer).ThenInclude(c => c.Tier)
            .Include(b => b.WashBay)
            .Include(b => b.Branch)
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        if (booking.Status != BookingStatus.InProgress)
            throw new AppException("Only InProgress bookings can be completed.", 400);

        // Update booking status
        booking.Status = BookingStatus.Completed;
        booking.UpdatedAt = DateTime.UtcNow;

        // Release wash bay
        booking.WashBay.Status = WashBayStatus.Available;

        // Calculate and award loyalty points
        var customer = booking.Customer;
        var pointsEarned = (int)Math.Floor(booking.TotalPrice / 1000m * customer.Tier.PointRate);

        customer.TotalWashes += 1;
        customer.TotalSpent += booking.TotalPrice;
        customer.TotalPoints += pointsEarned;
        customer.LifetimePoints += pointsEarned;

        // Auto-evaluate tier upgrade/downgrade
        await EvaluateTierAsync(customer);

        await _context.SaveChangesAsync();

        return MapToResponse(booking);
    }

    public async Task<StaffBookingResponse> CancelBookingAsync(Guid bookingId, string? reason)
    {
        var booking = await GetBookingWithIncludes(bookingId);

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            throw new AppException("Only Pending or Confirmed bookings can be cancelled.", 400);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(booking);
    }

    private async Task EvaluateTierAsync(Domain.Entities.Customer customer)
    {
        var tiers = await _context.Tiers
            .OrderByDescending(t => t.MinPointsRequired)
            .ToListAsync();

        // UPGRADE: find highest tier where customer qualifies by lifetime points
        var upgradeTier = tiers.FirstOrDefault(t => customer.LifetimePoints >= t.MinPointsRequired);

        // DOWNGRADE: check maintenance points (sum of points earned in last 90 days)
        var currentTier = customer.Tier;
        if (currentTier.MaintenancePoints > 0)
        {
            var cutoffDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-90));
            var recentPoints = await _context.Bookings
                .Where(b => b.CustomerId == customer.Id
                            && b.Status == BookingStatus.Completed
                            && b.BookingDate >= cutoffDate)
                // TODO: Uses current tier's PointRate for all past bookings. Ideally each Booking
                // should store PointsEarned at completion time for accurate historical calculation.
                .SumAsync(b => (int)Math.Floor(b.TotalPrice / 1000m * currentTier.PointRate));

            if (recentPoints < currentTier.MaintenancePoints)
            {
                // Find the highest tier the customer can maintain
                var downgradeTier = tiers.FirstOrDefault(t => recentPoints >= t.MaintenancePoints)
                                    ?? tiers.Last(); // fallback to Member

                // Only downgrade, never upgrade via maintenance check
                if (downgradeTier.MinPointsRequired < currentTier.MinPointsRequired)
                {
                    upgradeTier = downgradeTier;
                }
            }
        }

        if (upgradeTier != null && upgradeTier.Id != customer.TierId)
        {
            customer.TierId = upgradeTier.Id;
            customer.Tier = upgradeTier;
        }
    }

    private async Task<Domain.Entities.Booking> GetBookingWithIncludes(Guid bookingId)
    {
        return await _context.Bookings
            .Include(b => b.WashPackage)
            .Include(b => b.Vehicle)
            .Include(b => b.Customer)
            .Include(b => b.WashBay)
            .Include(b => b.Branch)
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);
    }

    private static StaffBookingResponse MapToResponse(Domain.Entities.Booking booking)
    {
        return new StaffBookingResponse
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            WashPackageName = booking.WashPackage.Name,
            DurationMinutes = booking.WashPackage.DurationMinutes,
            BookingDate = booking.BookingDate,
            StartTime = booking.StartTime,
            VehiclePlate = booking.Vehicle.LicensePlate ?? string.Empty,
            VehicleName = booking.Vehicle.VehicleName,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            QrData = booking.QrData,
            CreatedAt = booking.CreatedAt,
            CustomerName = booking.Customer.FullName,
            BayName = booking.WashBay.Name,
            BranchName = booking.Branch.BranchName,
            UpdatedAt = booking.UpdatedAt
        };
    }
}
