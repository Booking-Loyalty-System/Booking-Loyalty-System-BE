using Application.Common;
using Application.DTOs.Loyalty;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class LoyaltyService : ILoyaltyService
{
    private readonly IApplicationDbContext _context;
    private readonly LoyaltyOptions _options;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public LoyaltyService(IApplicationDbContext context, LoyaltyOptions options, IEmailService emailService, INotificationService notificationService)
    {
        _context = context;
        _options = options;
        _emailService = emailService;
        _notificationService = notificationService;
    }

    public async Task AwardPointsForBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var freeWashRewardId = Guid.Parse("10000000-0000-0000-0000-000000000099");
        // Serializable so two concurrent completions of the same booking cannot both
        // pass the "already earned?" check and double-award.
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken: cancellationToken);

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null)
            return; // Nothing to award; caller already validated existence in normal flow.

        // Idempotency: one Earn row per booking. If it already exists, this is a no-op.
        var alreadyEarned = await _context.PointHistories
            .AnyAsync(h => h.BookingId == bookingId && h.TransactionType == LoyaltyTransactionType.Earn, cancellationToken);
        if (alreadyEarned)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        var customer = await _context.Customers
            .Include(c => c.Tier)
            .Include(u => u.User)
            .FirstOrDefaultAsync(c => c.Id == booking.CustomerId, cancellationToken)
            ?? throw new AppException("Customer profile not found.", 404);

        var oldTierId = customer.TierId;
        var oldTierName = customer.Tier?.TierName ?? "Thành viên mới";
        
        var points = (int)Math.Floor(booking.TotalPrice * _options.PointsPerCurrencyUnit * customer.Tier.PointRate);
        var now = DateTime.UtcNow;

        var point = await GetOrCreatePointAsync(customer.UserId, now, cancellationToken);

        // Update the point balance (spendable + lifetime) and the customer's CRM stats.
        point.AvailablePoints += points;
        point.TotalPoints += points; // Tổng điểm lũy kế trọn đời nằm ở đây!
        point.UpdatedAt = now;
        customer.TotalWashes += 1;
        customer.TotalSpent += booking.TotalPrice;

        // SỬA TẠI ĐÂY: Dùng point.TotalPoints thay vì customer.TotalPoints
        var eligibleTier = await _context.Tiers
            .Where(t => t.MinPointsRequired <= point.TotalPoints)
            .OrderByDescending(t => t.MinPointsRequired)
            .FirstOrDefaultAsync(cancellationToken);
        
        bool isUpgraded = false;
        string newTierName = string.Empty;
        
        if (eligibleTier != null && eligibleTier.Id != oldTierId)
        {
            customer.TierId = eligibleTier.Id;
            newTierName = eligibleTier.TierName;
            isUpgraded = true;
        }
        
        // SỬA TẠI ĐÂY: Xóa bỏ dòng thừa 'var earn = new LoyaltyTransaction' gây lỗi compile
        var earn = new PointHistory
        {
            Id = Guid.NewGuid(),
            PointId = point.Id,
            TransactionType = LoyaltyTransactionType.Earn,
            Amount = points,
            BalanceAfter = point.AvailablePoints,
            BookingId = booking.Id,
            Description = $"Earned from booking {booking.BookingCode}",
            CreatedAt = now,
            ExpiryDate = now.AddMonths(_options.PointLifetimeMonths)
        };

        _context.PointHistories.Add(earn);
        bool isFreeWashAwarded = false;
        if (customer.TotalWashes > 0 && customer.TotalWashes % 7 == 0)
        {
            // Kiểm tra xem phần thưởng này có đang active trong DB không
            var rewardExists = await _context.Rewards
                .AnyAsync(r => r.Id == freeWashRewardId && r.IsActive, cancellationToken);

            if (rewardExists)
            {
                // Tạo bản ghi quy đổi phần thưởng cho khách hàng (Tặng Voucher)
                var redemption = new RewardRedemption
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    RewardId = freeWashRewardId,
                    CreatedAt = now,
                    Status = RedemptionStatus.Pending,
                    // Điền thêm các trường bắt buộc khác của class RewardRedemption nếu có ở đây...
                };

                _context.RewardRedemptions.Add(redemption);
                isFreeWashAwarded = true;
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        // SỬA TẠI ĐÂY: Dùng point.TotalPoints để gửi email thông báo
        if (isUpgraded && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
        {
            await SendUpgradeEmailSafeAsync(customer.User.Email, oldTierName, newTierName, point.TotalPoints);
        }
        
        if (isFreeWashAwarded && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
        {
            await _notificationService.SendNotificationToCustomerAsync(
                customer.Id,
                "Quà tặng tri ân độc quyền! 🎉",
                "Bạn đã hoàn thành mốc 7 lượt dịch vụ. Hệ thống đã gửi tặng bạn 1 Voucher Rửa xe miễn phí vào kho quà!",
                freeWashRewardId,
                "Loyalty"
            );
        }
    }

  public async Task ApplyNoShowPenaltyAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        // Mở transaction để đảm bảo tính đồng bộ cô lập (Idempotency)
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken: cancellationToken);

        // 1. Tìm thông tin Booking
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
            
        // Chỉ phạt nếu lịch đặt thực sự tồn tại và ở trạng thái NoShow
        if (booking is null || booking.Status != BookingStatus.NoShow)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        // 2. Kiểm tra chống phạt trùng (Idempotency) trước khi gọi tiếp DB để tối ưu hiệu năng
        var alreadyPenalized = await _context.PointHistories
            .AnyAsync(h => h.BookingId == bookingId && h.TransactionType == LoyaltyTransactionType.Penalty, cancellationToken);
        if (alreadyPenalized)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        // 3. Tìm Customer kèm thông tin Tier để lấy PointRate (Hệ số nhân theo hạng thẻ)
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.Id == booking.CustomerId, cancellationToken);
            
        if (customer is null || customer.Tier is null)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        // 4. TÍNH ĐIỂM PHẠT: Gấp đôi số điểm đáng lẽ nhận được (Bê nguyên công thức từ hàm Award sang)
        var expectedPoints = (int)Math.Floor(booking.TotalPrice * _options.PointsPerCurrencyUnit * customer.Tier.PointRate);
        var penalty = expectedPoints * 2; 
        
        // Nếu số điểm phạt tính ra <= 0 (Do đơn hàng 0đ hoặc hệ số lỗi) thì bỏ qua
        if (penalty <= 0)
        {
            await transaction.CommitAsync(cancellationToken);
            return; 
        }

        // 5. Lấy ví điểm hiện tại của khách
        var point = await _context.Points
            .FirstOrDefaultAsync(p => p.UserId == customer.UserId, cancellationToken);

        // Khách không có ví điểm hoặc điểm hiện tại đang bằng 0 thì không có gì để trừ
        var available = point?.AvailablePoints ?? 0;
        if (point is null || available <= 0)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        var now = DateTime.UtcNow;
        
        // 6. XỬ LÝ TRỪ ĐIỂM VÀ GIỚI HẠN VỀ 0:
        // Math.Min sẽ chọn số nhỏ hơn. Nếu Điểm phạt lớn hơn Điểm hiện có, 
        // nó lấy luôn Điểm hiện có để trừ, đưa ví về bằng đúng 0 (không bị âm).
        var deduct = Math.Min(penalty, available); 
        point.AvailablePoints -= deduct;
        point.UpdatedAt = now;

        // Ghi nhận lịch sử ví điểm (Ledger)
        var ledger = new PointHistory
        {
            Id = Guid.NewGuid(),
            PointId = point.Id,
            TransactionType = LoyaltyTransactionType.Penalty,
            Amount = -deduct,
            BalanceAfter = point.AvailablePoints,
            BookingId = booking.Id,
            Description = $"No-show penalty (Double expected points: {penalty}) for booking {booking.BookingCode}",
            CreatedAt = now
        };

        _context.PointHistories.Add(ledger);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task SendUpgradeEmailSafeAsync(string toEmail, string oldTier, string newTier, int currentPoints)
    {
            string subject = "🎉 Chúc mừng bạn đã thăng hạng thành viên!";
            string body = $@"
            <h2>Chào bạn,</h2>
            <p>Chúc mừng bạn đã tích luỹ đủ điểm và chính thức thăng hạng từ <b>{oldTier}</b> lên <b>{newTier}</b>!</p>
            <p>Số điểm hiện tại của bạn là: <b>{currentPoints} điểm</b>.</p>
            <p>Đăng nhập vào ứng dụng ngay để khám phá các Voucher và đặc quyền mới dành riêng cho hạng {newTier} nhé.</p>
            <br/>
            <p>Cảm ơn bạn đã đồng hành cùng chúng tôi!</p>
        ";

            await _emailService.SendEmailAsync(toEmail, subject, body);
    }
    
    public async Task<LoyaltyBalanceResponse> GetBalanceAsync(Guid userId)
    {
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var point = await _context.Points.FirstOrDefaultAsync(p => p.UserId == userId);

        return new LoyaltyBalanceResponse
        {
            TotalPoints = point?.AvailablePoints ?? 0,
            LifetimePoints = point?.TotalPoints ?? 0,
            TotalWashes = customer.TotalWashes,
            TotalSpent = customer.TotalSpent,
            Tier = customer.Tier?.TierName ?? string.Empty
        };
    }

    public async Task<List<LoyaltyTransactionResponse>> GetHistoryAsync(Guid userId)
    {
        var point = await _context.Points.FirstOrDefaultAsync(p => p.UserId == userId);
        if (point is null)
            return new List<LoyaltyTransactionResponse>();

        return await _context.PointHistories
            .Where(h => h.PointId == point.Id)
            .OrderByDescending(h => h.CreatedAt)
            .Select(h => new LoyaltyTransactionResponse
            {
                Id = h.Id,
                Type = h.TransactionType.ToString(),
                Points = h.Amount,
                BalanceAfter = h.BalanceAfter,
                BookingId = h.BookingId,
                Description = h.Description,
                CreatedAt = h.CreatedAt,
                ExpiresAt = h.ExpiryDate
            })
            .ToListAsync();
    }

    /// <summary>Loads the user's Point balance row, creating it on first use.</summary>
    private async Task<Point> GetOrCreatePointAsync(Guid userId, DateTime now, CancellationToken ct)
    {
        var point = await _context.Points.FirstOrDefaultAsync(p => p.UserId == userId, ct);
        if (point is not null)
            return point;

        point = new Point
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AvailablePoints = 0,
            TotalPoints = 0,
            UpdatedAt = now
        };
        _context.Points.Add(point);
        return point;
    }
}
