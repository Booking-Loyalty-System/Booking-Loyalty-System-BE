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
    private readonly INotificationService _notificationService;

    public StaffBookingService(IApplicationDbContext context, ILoyaltyService loyaltyService, IHubContext<BookingHub> hubContext, INotificationService notificationService)
    {
        _context = context;
        _loyaltyService = loyaltyService;
        _hubContext = hubContext;
        _notificationService = notificationService;
    }

    public async Task<BookingResponseData> GetBookingByQrPayloadAsync(string qrPayload)
    {
        if (string.IsNullOrWhiteSpace(qrPayload))
            throw new AppException("Mã QR không hợp lệ hoặc rỗng.", 400);

        qrPayload = qrPayload.Trim();
        bool isGuid = Guid.TryParse(qrPayload, out Guid bookingId);

        var booking = await _context.Bookings
                          .Include(b => b.Vehicle)
                          .Include(b => b.WashPackage)
                          .Include(b => b.BranchTimeSlot)
                          .ThenInclude(bts => bts.TimeSlot)
                          .FirstOrDefaultAsync(b => 
                              (isGuid && b.Id == bookingId) || 
                              b.BookingCode == qrPayload)
                      ?? throw new AppException("Không tìm thấy lịch đặt từ mã QR này.", 404);

        return await BuildResponseAsync(booking);
    }
    
    public async Task<List<BookingResponseData>> GetByDateAsync(Guid userId, DateOnly date)
    {
        // 1. Tìm thông tin Staff dựa vào userId để lấy BranchId
        var staff = await _context.Staffs
                        .FirstOrDefaultAsync(s => s.UserId == userId)
                    ?? throw new AppException("Staff profile not found.", 404);

        // 2. Lấy danh sách Bookings của ngày đó VÀ phải thuộc Chi nhánh của Staff này
        var bookings = await _context.Bookings
            .Where(b => b.BookingDate == date && b.BranchTimeSlot.BranchId == staff.BranchId) // Thêm điều kiện lọc theo BranchId ở đây
            .Include(b => b.Vehicle)
            .Include(b => b.WashPackage)
            .Include(b => b.BranchTimeSlot)
            .ThenInclude(bts => bts.TimeSlot)
            .OrderBy(b => b.BranchTimeSlot.TimeSlot.StartTime) 
            .ToListAsync();

        // 3. Gom danh sách ID để lấy điểm Loyalty (giữ nguyên logic cũ của bạn)
        var ids = bookings.Select(b => b.Id).ToList();

        var pointsByBooking = await _context.PointHistories
            .Where(h => h.TransactionType == LoyaltyTransactionType.Earn
                         && h.BookingId != null
                         && ids.Contains(h.BookingId.Value))
            .ToDictionaryAsync(h => h.BookingId!.Value, h => h.Amount);

        // 4. Map dữ liệu trả về
        return bookings
            .Select(b => MapToResponseData(
                b,
                pointsByBooking.TryGetValue(b.Id, out var points) ? points : null))
            .ToList();
    }

    // 1. CONFIRM
    public async Task<BookingResponseData> ConfirmAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "confirm", BookingStatus.Pending);

        booking.Status = BookingStatus.Confirmed;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
       // await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 2. CHECK-IN (Chỉ lưu staffId, không gán khoang)
    public async Task<BookingResponseData> CheckInAsync(Guid bookingId, Guid staffId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "check-in", BookingStatus.Confirmed);

        if (staffId == Guid.Empty)
            throw new AppException("staffId is required to check-in.", 400);
        
        booking.StaffId = staffId;
        booking.Status = BookingStatus.CheckedIn;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 3. QUEUE (Lưu BayId, đẩy xe vào khoang)
    public async Task<BookingResponseData> QueueAsync(Guid bookingId, Guid washBayId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "queue", BookingStatus.Confirmed, BookingStatus.CheckedIn);

        if (washBayId == Guid.Empty)
            throw new AppException("washBayId is required to queue a booking.", 400);

        var washBay = await _context.WashBays.FindAsync(washBayId);
        if (washBay == null) throw new AppException("Wash Bay not found.", 404);

        booking.BayId = washBayId; // Gán xe vào khoang
        washBay.Status = WashBayStatus.InProgress;

        booking.Status = BookingStatus.Queued;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 4. START SERVICE (Chỉ cập nhật trạng thái)
    public async Task<BookingResponseData> StartServiceAsync(Guid bookingId, Guid? bayId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "start", BookingStatus.Confirmed, BookingStatus.CheckedIn, BookingStatus.Queued);
        if (bayId.HasValue && bayId != booking.BayId)
        {
            // 1. Giải phóng khoang cũ (nếu có)
            if (booking.BayId.HasValue) 
                await CheckAndReleaseWashBayAsync(booking.BayId, booking.Id);

            // 2. Gán khoang mới
            var washBay = await _context.WashBays.FindAsync(bayId.Value) 
                          ?? throw new AppException("Khoang rửa không tồn tại.", 404);
        
            booking.BayId = bayId.Value;
            washBay.Status = WashBayStatus.InProgress;
        }
        booking.Status = BookingStatus.InProgress;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 5. COMPLETE (Chỉ cập nhật trạng thái và giải phóng khoang)
    public async Task<BookingResponseData> CompleteServiceAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "finish", BookingStatus.InProgress);

        booking.Status = BookingStatus.Completed;
        booking.UpdatedAt = DateTime.UtcNow;

        // 🌟 THÊM ĐOẠN NÀY: LOGIC TỰ ĐỘNG BẮT ĐẦU XE TIẾP THEO
        if (booking.BayId.HasValue) 
        {
            var nextBooking = await _context.Bookings
                .Where(b => b.BayId == booking.BayId && b.Status == BookingStatus.Queued)
                .OrderBy(b => b.CreatedAt) // Lấy xe vào hàng đợi sớm nhất (FIFO)
                .FirstOrDefaultAsync();

            if (nextBooking != null)
            {
                nextBooking.Status = BookingStatus.InProgress;
                nextBooking.UpdatedAt = DateTime.UtcNow;

                await NotifyCustomerAsync(nextBooking.CustomerId, nextBooking.Id, nextBooking.Status);
            }
        }
        
        await CheckAndReleaseWashBayAsync(booking.BayId, booking.Id);

        await _context.SaveChangesAsync();
        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 6. CHECK-OUT (Thanh toán, cộng điểm, tính lại hạng thành viên)
    public async Task<BookingResponseData> CheckOutAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "checkout", BookingStatus.Completed);

        booking.Status = BookingStatus.CheckedOut;
        booking.UpdatedAt = DateTime.UtcNow;

        // Kiểm tra giải phóng khoang rửa xe (nếu bước trước đó chưa giải phóng)
        await CheckAndReleaseWashBayAsync(booking.BayId, booking.Id);
        await _context.SaveChangesAsync();

        // Xử lý Loyalty & Hạng thành viên
        await _loyaltyService.AwardPointsForBookingAsync(bookingId);

        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstAsync(c => c.Id == booking.CustomerId);
        await EvaluateTierAsync(customer);
        
        await _context.SaveChangesAsync();
        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 7. CANCEL (Chỉ cập nhật trạng thái và giải phóng khoang)
    public async Task<BookingResponseData> CancelAsync(Guid bookingId, string? reason)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "cancel", BookingStatus.Pending, BookingStatus.Confirmed, BookingStatus.CheckedIn, BookingStatus.Queued);

        booking.Status = BookingStatus.Cancelled;
        booking.CancellationReason = reason;
        booking.UpdatedAt = DateTime.UtcNow;

        await CheckAndReleaseWashBayAsync(booking.BayId, booking.Id);

        // Staff hủy thì nhả voucher đã áp về Pending để khách dùng lại (No-show thì KHÔNG nhả).
        var redemption = await _context.RewardRedemptions
            .FirstOrDefaultAsync(r => r.BookingId == booking.Id && r.Status == RedemptionStatus.Fulfilled);
        if (redemption != null)
        {
            redemption.Status = RedemptionStatus.Pending;
            redemption.FulfilledAt = null;
            redemption.BookingId = null;
        }

        await _context.SaveChangesAsync();
        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // 8. NO-SHOW (Chỉ cập nhật trạng thái và giải phóng khoang)
    public async Task<BookingResponseData> NoShowAsync(Guid bookingId)
    {
        var booking = await LoadAsync(bookingId);
        EnsureStatus(booking, "no-show", BookingStatus.Confirmed, BookingStatus.CheckedIn);

        booking.Status = BookingStatus.NoShow;
        booking.CancellationReason = "Customer did not show up.";
        booking.UpdatedAt = DateTime.UtcNow;

        await CheckAndReleaseWashBayAsync(booking.BayId, booking.Id);

        await _context.SaveChangesAsync();

        // Deduct the no-show penalty (spendable points, clamped at 0). Idempotent.
        await _loyaltyService.ApplyNoShowPenaltyAsync(booking.Id);

        await NotifyCustomerAsync(booking.CustomerId, booking.Id, booking.Status);

        return await BuildResponseAsync(booking);
    }

    // --- CÁC HÀM PRIVATE HELPER ---

    private async Task CheckAndReleaseWashBayAsync(Guid? washBayId, Guid currentBookingId)
    {
        if (!washBayId.HasValue) return;

        // Một khoang vẫn Busy nếu còn xe CheckedIn, Queued hoặc InProgress (loại trừ xe hiện tại)
        bool hasActiveBookings = await _context.Bookings
            .AnyAsync(b => b.BayId == washBayId 
                        && b.Id != currentBookingId 
                        && (b.Status == BookingStatus.CheckedIn || 
                            b.Status == BookingStatus.Queued || 
                            b.Status == BookingStatus.InProgress));

        if (!hasActiveBookings)
        {
            var washBay = await _context.WashBays.FindAsync(washBayId);
            if (washBay != null)
            {
                washBay.Status = WashBayStatus.Available; 
                _context.WashBays.Update(washBay);
            }
        }
    }

    private async Task NotifyCustomerAsync(Guid customerId, Guid bookingId, BookingStatus status)
    {
        string title = "Cập nhật trạng thái lịch rửa xe";
        string message = $"Lịch đặt {bookingId.ToString().Substring(0,8)} của bạn đã chuyển sang trạng thái: {status}";

        // 1. Lưu vào Database để khách xem lại trong mục "Thông báo"
        await _notificationService.CreateNotificationAsync(customerId, title, message, "Booking");

        // 2. Gửi Real-time qua SignalR
        await _hubContext.Clients.Group($"Customer_{customerId}").SendAsync("BookingStatusChanged", new 
        {
            BookingId = bookingId,
            Status = status.ToString()
        });
    }

    private async Task<Booking> LoadAsync(Guid bookingId)
    {
        return await _context.Bookings
                   .Include(b => b.Vehicle)
                   .Include(b => b.WashPackage)
                   .Include(b => b.BranchTimeSlot)
                   .ThenInclude(bts => bts.TimeSlot)
                   .FirstOrDefaultAsync(b => b.Id == bookingId)
               ?? throw new AppException("Booking not found.", 404);
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
        var points = await _context.PointHistories
            .Where(h => h.BookingId == booking.Id && h.TransactionType == LoyaltyTransactionType.Earn)
            .Select(h => (int?)h.Amount)
            .FirstOrDefaultAsync();

        return MapToResponseData(booking, points);
    }

    private async Task EvaluateTierAsync(Customer customer)
    {
        var tiers = await _context.Tiers
            .OrderByDescending(t => t.MinPointsRequired)
            .ToListAsync();

        var point = await _context.Points.FirstOrDefaultAsync(p => p.UserId == customer.UserId);
        var lifetimePoints = point?.TotalPoints ?? 0;

        var qualifiedTier = tiers.FirstOrDefault(t => lifetimePoints >= t.MinPointsRequired);

        if (qualifiedTier != null && qualifiedTier.MinPointsRequired > customer.Tier.MinPointsRequired)
        {
            customer.TierId = qualifiedTier.Id;
            return;
        }

        var cutoff = DateTime.UtcNow.AddDays(-90);
        var recentPoints = point is null ? 0 : await _context.PointHistories
            .Where(h => h.PointId == point.Id
                      && h.TransactionType == LoyaltyTransactionType.Earn
                      && h.CreatedAt >= cutoff)
            .SumAsync(h => h.Amount);

        if (recentPoints < customer.Tier.MaintenancePoints)
        {
            var newTier = tiers.FirstOrDefault(t =>
                lifetimePoints >= t.MinPointsRequired && recentPoints >= t.MaintenancePoints);

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
            BranchId = booking.BranchTimeSlot?.BranchId ?? Guid.Empty,
            BookingDate = booking.BookingDate.ToString("yyyy-MM-dd"),
            StartTime = booking.BranchTimeSlot?.TimeSlot?.StartTime.ToString("HH\\:mm") ?? "00:00", 
            Status = booking.Status.ToString(),
            TotalAmount = booking.TotalPrice,
            PointsEarned = pointsEarned,
            CreatedAt = booking.CreatedAt.ToString("o"),
            BayId = booking.BayId
        };
    }
}