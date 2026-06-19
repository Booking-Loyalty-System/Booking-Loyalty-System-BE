using Application.DTOs.Booking;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StaffBookingService : IStaffBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly ILoyaltyService _loyaltyService;
    private readonly IHubContext<BookingHub> _hubContext;
    public StaffBookingService(IApplicationDbContext context, ILoyaltyService loyaltyService, IHubContext<BookingHub> hubContext)
    {
        _context = context;
        _loyaltyService = loyaltyService;
        _hubContext = hubContext;
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

    public async Task<BookingResponseData> UpdateStatusAsync(Guid bookingId, UpdateBookingStatusRequest request)
    {
        var booking = await LoadAsync(bookingId);
        
        // Lưu lại trạng thái cũ để đối chiếu nếu cần
        var oldStatus = booking.Status; 

        switch (request.TargetStatus)
        {
            case BookingStatus.Confirmed:
                EnsureStatus(booking, "confirm", BookingStatus.Pending);
                booking.Status = BookingStatus.Confirmed;
                break;
            
            case BookingStatus.CheckedIn:
                EnsureStatus(booking, "check-in", BookingStatus.Confirmed);
                booking.Status = BookingStatus.CheckedIn;
                break;

            case BookingStatus.Queued:
                // Cho phép xếp hàng từ lúc Confirmed (đến tiệm) hoặc CheckedIn đều được
                EnsureStatus(booking, "queue", BookingStatus.Confirmed, BookingStatus.CheckedIn);
                booking.Status = BookingStatus.Queued;
                break;

            case BookingStatus.InProgress:
                // 🔥 SỬA TẠI ĐÂY: Cho phép bấm nút Start trực tiếp từ Confirmed, CheckedIn hoặc Queued
                EnsureStatus(booking, "start", BookingStatus.Confirmed, BookingStatus.CheckedIn, BookingStatus.Queued);
    
                if (!request.StaffId.HasValue || request.StaffId == Guid.Empty)
                    throw new AppException("staffId is required to start a service.", 400);

                var isValidStaff = await _context.Users
                    .AnyAsync(u => u.Id == request.StaffId && u.Role == UserRole.Staff && u.IsActive);
                if (!isValidStaff)
                    throw new AppException("Assigned staff not found or is not an active staff member.", 400);

                booking.AssignedStaffId = request.StaffId;
                booking.Status = BookingStatus.InProgress;
                break;

            case BookingStatus.Completed:
                EnsureStatus(booking, "finish", BookingStatus.InProgress);
                booking.Status = BookingStatus.Completed;
                if (booking.WashBayTimeSlot?.TimeSlot != null)
                    booking.WashBayTimeSlot.Status = TimeSlotStatus.Completed;
                break;

            case BookingStatus.CheckedOut:
                EnsureStatus(booking, "checkout", BookingStatus.Completed);
                booking.Status = BookingStatus.CheckedOut;
                
                if (!string.IsNullOrWhiteSpace(request.Reason))
                {
                    booking.CancellationReason = request.Reason; // Sẽ lưu chuỗi kiểu: "Paid via Cash" hoặc "Paid via Bank Transfer"
                }
                
                // Cộng điểm Loyalty thưởng khi khách xuất xưởng thanh toán
                await _loyaltyService.AwardPointsForBookingAsync(bookingId);
                break;

            case BookingStatus.Cancelled:
                EnsureStatus(booking, "cancel", 
                    BookingStatus.Pending, BookingStatus.Confirmed, BookingStatus.CheckedIn, BookingStatus.Queued);
                
                booking.Status = BookingStatus.Cancelled;
                booking.CancellationReason = request.Reason;
                ReleaseTimeSlot(booking);
                break;

            case BookingStatus.NoShow:
                EnsureStatus(booking, "no-show", BookingStatus.Confirmed, BookingStatus.CheckedIn);
                
                booking.Status = BookingStatus.NoShow;
                booking.CancellationReason = request.Reason ?? "Customer did not show up.";
                ReleaseTimeSlot(booking);
                break;

            
            default:
                throw new AppException($"Invalid target status: {request.TargetStatus}", 400);
        }
        booking.Status = BookingStatus.CheckedOut;
        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Award loyalty points on checkout (idempotent — one Earn row per booking).
        await _loyaltyService.AwardPointsForBookingAsync(bookingId);

        // Evaluate tier upgrade/downgrade after earning points.
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstAsync(c => c.Id == booking.CustomerId);
        await EvaluateTierAsync(customer);
        await _context.SaveChangesAsync();

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

        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        await _hubContext.Clients.Group($"Customer_{booking.CustomerId}").SendAsync("BookingStatusChanged", new 
        {
            BookingId = booking.Id,
            Status = booking.Status.ToString()
        });
        
        return await BuildResponseAsync(booking);
    }

    private async Task<Booking> LoadAsync(Guid bookingId)
    {
        return await _context.Bookings
                   .Include(b => b.Vehicle)
                   .Include(b => b.WashPackage)
                   .Include(b => b.WashBayTimeSlot)
                   .ThenInclude(wbts => wbts.TimeSlot)
                   .FirstOrDefaultAsync(b => b.Id == bookingId)
               ?? throw new AppException("Booking not found.", 404);
    }

    /// <summary>Releases the reserved bay/time slot so the window becomes bookable again.</summary>
    private void ReleaseTimeSlot(Booking booking)
    {
        if (booking.WashBayTimeSlot != null)
        {
            _context.WashBayTimeSlots.Remove(booking.WashBayTimeSlot);
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

    private async Task EvaluateTierAsync(Customer customer)
    {
        var tiers = await _context.Tiers
            .OrderByDescending(t => t.MinPointsRequired)
            .ToListAsync();

        // Upgrade: find the highest tier the customer qualifies for by lifetime points.
        var qualifiedTier = tiers.FirstOrDefault(t => customer.LifetimePoints >= t.MinPointsRequired);

        if (qualifiedTier != null && qualifiedTier.MinPointsRequired > customer.Tier.MinPointsRequired)
        {
            customer.TierId = qualifiedTier.Id;
            return; // Upgrade takes priority — skip downgrade check.
        }

        // Downgrade: check points earned in the last 90 days against maintenance threshold.
        var cutoff = DateTime.UtcNow.AddDays(-90);
        var recentPoints = await _context.LoyaltyTransactions
            .Where(lt => lt.CustomerId == customer.Id
                      && lt.Type == LoyaltyTransactionType.Earn
                      && lt.CreatedAt >= cutoff)
            .SumAsync(lt => lt.Points);

        if (recentPoints < customer.Tier.MaintenancePoints)
        {
            // Find the highest tier whose maintenance the customer can satisfy.
            var newTier = tiers.FirstOrDefault(t =>
                customer.LifetimePoints >= t.MinPointsRequired && recentPoints >= t.MaintenancePoints);

            if (newTier != null && newTier.Id != customer.TierId)
                customer.TierId = newTier.Id;
        }
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
