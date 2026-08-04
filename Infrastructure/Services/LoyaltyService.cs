using Application.Common;
using Application.DTOs.Loyalty;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Services;

public class LoyaltyService : ILoyaltyService
{
    private readonly IApplicationDbContext _context;
    private readonly LoyaltyOptions _options;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<LoyaltyService> _logger;

    public LoyaltyService(IApplicationDbContext context, LoyaltyOptions options, IEmailService emailService, INotificationService notificationService, ILogger<LoyaltyService> logger)
    {
        _context = context;
        _options = options;
        _emailService = emailService;
        _notificationService = notificationService;
        _logger = logger;
    }

    //public async Task AwardPointsForBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    //{
    //    var freeWashRewardId = Guid.Parse("10000000-0000-0000-0000-000000000099");
    //    // Serializable so two concurrent completions of the same booking cannot both
    //    // pass the "already earned?" check and double-award.
    //    await using var transaction = await _context.BeginTransactionAsync(cancellationToken: cancellationToken);

    //    var booking = await _context.Bookings
    //        .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
    //    if (booking is null)
    //        return; // Nothing to award; caller already validated existence in normal flow.

    //    // Idempotency: one Earn row per booking. If it already exists, this is a no-op.
    //    var alreadyEarned = await _context.PointHistories
    //        .AnyAsync(h => h.BookingId == bookingId && h.TransactionType == LoyaltyTransactionType.Earn, cancellationToken);
    //    if (alreadyEarned)
    //    {
    //        await transaction.CommitAsync(cancellationToken);
    //        return;
    //    }

    //    var customer = await _context.Customers
    //        .Include(c => c.Tier)
    //        .Include(u => u.User)
    //        .FirstOrDefaultAsync(c => c.Id == booking.CustomerId, cancellationToken)
    //        ?? throw new AppException("Customer profile not found.", 404);

    //    var oldTierId = customer.TierId;
    //    var oldTierName = customer.Tier?.TierName ?? "Thành viên mới";

    //    var points = (int)Math.Floor(booking.TotalPrice * _options.PointsPerCurrencyUnit * customer.Tier.PointRate);
    //    var now = DateTime.UtcNow;

    //    var point = await GetOrCreatePointAsync(customer.UserId, now, cancellationToken);

    //    // Update the point balance (spendable + lifetime) and the customer's CRM stats.
    //    point.AvailablePoints += points;
    //    point.TotalPoints += points; // Tổng điểm lũy kế trọn đời nằm ở đây!
    //    point.UpdatedAt = now;
    //    customer.TotalWashes += 1;
    //    customer.CurrentCycleWashes += 1; // Thẻ tích rửa: cộng dồn trong chu kỳ hiện tại.
    //    customer.TotalSpent += booking.TotalPrice;

    //    // === GỘP LOGIC HẠNG (nguồn duy nhất) ===
    //    // Nâng hạng theo điểm lũy kế trọn đời; GIỮ/HẠ hạng theo SỐ BOOKING hoàn tất trong ~30 ngày gần nhất.
    //    // Quy tắc thuần & idempotent (không dao động): hạng = hạng CAO NHẤT thỏa CẢ HAI điều kiện:
    //    //   TotalPoints (lũy kế) >= MinPointsRequired  VÀ  số booking 30 ngày >= MaintenanceBookings.
    //    var since = DateOnly.FromDateTime(now.AddDays(-30));
    //    var otherDoneBookings = await _context.Bookings.CountAsync(b =>
    //        b.CustomerId == customer.Id
    //        && b.Id != booking.Id
    //        && b.BookingDate >= since
    //        && (b.Status == BookingStatus.Completed || b.Status == BookingStatus.CheckedOut),
    //        cancellationToken);
    //    var recentBookings = otherDoneBookings + 1; // + chính lượt đang checkout

    //    var oldTierMin = customer.Tier?.MinPointsRequired ?? 0;
    //    var allTiers = await _context.Tiers
    //        .OrderByDescending(t => t.MinPointsRequired)
    //        .ToListAsync(cancellationToken);
    //    var eligibleTier = PickTier(allTiers, point.TotalPoints, recentBookings);

    //    bool isUpgraded = false;
    //    string newTierName = string.Empty;

    //    // Checkout CHỈ nâng/giữ hạng, KHÔNG hạ. Việc hạ hạng do worker nền (TierMaintenanceService)
    //    // xử lý dần — mỗi lần chạy chỉ lùi 1 bậc — để tránh checkout lỡ nhảy xuống nhiều bậc cùng lúc.
    //    if (eligibleTier != null
    //        && eligibleTier.Id != oldTierId
    //        && eligibleTier.MinPointsRequired > oldTierMin)
    //    {
    //        customer.TierId = eligibleTier.Id;
    //        newTierName = eligibleTier.TierName;
    //        isUpgraded = true;
    //    }

    //    // SỬA TẠI ĐÂY: Xóa bỏ dòng thừa 'var earn = new LoyaltyTransaction' gây lỗi compile
    //    var earn = new PointHistory
    //    {
    //        Id = Guid.NewGuid(),
    //        PointId = point.Id,
    //        TransactionType = LoyaltyTransactionType.Earn,
    //        Amount = points,
    //        BalanceAfter = point.AvailablePoints,
    //        BookingId = booking.Id,
    //        Description = $"Earned from booking {booking.BookingCode}",
    //        CreatedAt = now,
    //        ExpiryDate = now.AddMonths(_options.PointLifetimeMonths)
    //    };

    //    _context.PointHistories.Add(earn);
    //    bool isFreeWashAwarded = false;
    //    // Thẻ tích rửa: cứ đủ 7 lần trong chu kỳ thì tặng 1 voucher rửa free rồi trừ 7 (reset chu kỳ,
    //    // giữ lại phần dư nếu vì lý do nào đó vượt 7). CurrentCycleWashes cho FE biết tiến độ "x/7".
    //    if (customer.CurrentCycleWashes >= 7)
    //    {
    //        // Kiểm tra xem phần thưởng này có đang active trong DB không
    //        var rewardExists = await _context.Rewards
    //            .AnyAsync(r => r.Id == freeWashRewardId && r.IsActive, cancellationToken);

    //        if (rewardExists)
    //        {
    //            // Tạo bản ghi quy đổi phần thưởng cho khách hàng (Tặng Voucher)
    //            var redemption = new RewardRedemption
    //            {
    //                Id = Guid.NewGuid(),
    //                CustomerId = customer.Id,
    //                RewardId = freeWashRewardId,
    //                CreatedAt = now,
    //                Status = RedemptionStatus.Pending,
    //                ExpiryDate = now.AddDays(30)
    //            };

    //            _context.RewardRedemptions.Add(redemption);
    //            customer.CurrentCycleWashes -= 7; // Reset chu kỳ tích rửa.
    //            isFreeWashAwarded = true;
    //        }
    //    }
    //    await _context.SaveChangesAsync(cancellationToken);
    //    await transaction.CommitAsync(cancellationToken);

    //    // SỬA TẠI ĐÂY: Dùng point.TotalPoints để gửi email thông báo
    //    if (isUpgraded && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
    //    {
    //        await SendUpgradeEmailSafeAsync(customer.User.Email, oldTierName, newTierName, point.TotalPoints);
    //    }

    //    if (isFreeWashAwarded && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
    //    {
    //        await _notificationService.SendNotificationToCustomerAsync(
    //            customer.Id,
    //            "Quà tặng tri ân độc quyền! 🎉",
    //            "Bạn đã hoàn thành mốc 7 lượt dịch vụ. Hệ thống đã gửi tặng bạn 1 Voucher Rửa xe miễn phí vào kho quà!",
    //            freeWashRewardId,
    //            "Loyalty"
    //        );
    //    }
    //}

    public async Task AwardPointsForBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken: cancellationToken);

        var booking = await _context.Bookings
        .Include(b => b.Reward)
        .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);

        if (booking is null)
            return;

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

        // Tier có thể null nếu hạng đã bị xoá khỏi hệ thống (admin có quyền xoá Tier).
        // Không để checkout ném NullReferenceException khiến khách mất điểm của lượt đó.
        var pointRate = customer.Tier?.PointRate ?? 1m;
        var points = (int)Math.Floor(booking.TotalPrice * _options.PointsPerCurrencyUnit * pointRate);
        var now = DateTime.UtcNow;

        var point = await GetOrCreatePointAsync(customer.UserId, now, cancellationToken);

        // Update the point balance (spendable + lifetime) and the customer's CRM stats.
        point.AvailablePoints += points;
        point.TotalPoints += points;
        point.UpdatedAt = now;

        customer.TotalSpent += booking.TotalPrice;

        bool isFreeWashBooking = booking.Reward != null && booking.Reward.IsFreeWash;
        customer.TotalWashes += 1;
        if (!isFreeWashBooking)
        {
            customer.CurrentCycleWashes += 1;
        }

        var since = DateOnly.FromDateTime(now.AddDays(-30));
        var otherDoneBookings = await _context.Bookings.CountAsync(b =>
            b.CustomerId == customer.Id
            && b.Id != booking.Id
            && b.BookingDate >= since
            && (b.Status == BookingStatus.Completed || b.Status == BookingStatus.CheckedOut)
            && (b.Reward == null || !b.Reward.IsFreeWash),
            cancellationToken);

        // Lượt rửa MIỄN PHÍ không được tính để giữ hạng — đồng bộ với cách tính CurrentCycleWashes
        // ở trên. Nếu tính, khách có thể dùng quà miễn phí để giữ hạng cao vô thời hạn.
        var recentBookings = otherDoneBookings + (isFreeWashBooking ? 0 : 1); // + chính lượt đang checkout

        var oldTierMin = customer.Tier?.MinPointsRequired ?? 0;
        var allTiers = await _context.Tiers
            .OrderByDescending(t => t.MinPointsRequired)
            .ToListAsync(cancellationToken);

        var eligibleTier = PickTier(allTiers, point.TotalPoints, recentBookings);

        bool isUpgraded = false;
        string newTierName = string.Empty;

        if (eligibleTier != null
            && eligibleTier.Id != oldTierId
            && eligibleTier.MinPointsRequired > oldTierMin)
        {
            customer.TierId = eligibleTier.Id;
            newTierName = eligibleTier.TierName;
            isUpgraded = true;
        }

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

        // === LOGIC TẶNG THƯỞNG RỬA XE SAU 7 LẦN ===
        bool isFreeWashAwarded = false;
        Guid? awardedRewardId = null;
        string awardedRewardName = string.Empty;
        DateTime? awardedExpiryDate = null;

        if (customer.CurrentCycleWashes >= 7)
        {
            // 1. Lấy 7 lượt rửa TRẢ TIỀN gần nhất (loại lượt dùng quà miễn phí).
            var last7Bookings = await _context.Bookings
                .Where(b => b.CustomerId == customer.Id
                         && b.Status == BookingStatus.CheckedOut
                         && (b.Reward == null || !b.Reward.IsFreeWash))
                .OrderByDescending(b => b.CreatedAt)
                .Take(7)
                .Select(b => new { b.WashPackageId, b.WashPackage.Price })
                .ToListAsync(cancellationToken);

            if (last7Bookings.Count == 7)
            {
                // 2. Tặng đúng gói khách dùng NHIỀU NHẤT trong chu kỳ; hoà thì ưu tiên gói đắt hơn.
                // (Trước đây tính theo giá TRUNG BÌNH với các mốc 200k/500k/1tr — lệch hoàn toàn với
                // giá gói thật (cao nhất 200k) nên FREE_VIP không bao giờ trao được, còn khách mua
                // gói cao mà có khuyến mãi lại bị tụt xuống quà gói thấp.)
                var targetPackageId = last7Bookings
                    .GroupBy(x => x.WashPackageId)
                    .OrderByDescending(g => g.Count())
                    .ThenByDescending(g => g.Max(x => x.Price))
                    .First().Key;

                var reward = await _context.Rewards
                    .FirstOrDefaultAsync(r => r.IsFreeWash && r.IsActive && r.WashPackageId == targetPackageId,
                        cancellationToken);

                if (reward == null)
                {
                    // Không có quà khớp gói (admin tắt/xoá, hoặc gói mới chưa cấu hình quà):
                    // lùi về quà rửa xe miễn phí rẻ nhất còn hiệu lực thay vì bỏ qua âm thầm.
                    reward = await _context.Rewards
                        .Where(r => r.IsFreeWash && r.IsActive)
                        .OrderBy(r => r.DiscountAmount)
                        .FirstOrDefaultAsync(cancellationToken);

                    _logger.LogWarning(
                        "Không có quà rửa xe miễn phí cấu hình cho gói {PackageId} (khách {CustomerId}). Dùng tạm quà {Fallback}.",
                        targetPackageId, customer.Id, reward?.Code ?? "(không có)");
                }

                if (reward != null)
                {
                    var expiryDate = now.AddDays(30);
                    var redemption = new RewardRedemption
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        RewardId = reward.Id,
                        CreatedAt = now,
                        Status = RedemptionStatus.Pending,
                        ExpiryDate = expiryDate
                    };

                    _context.RewardRedemptions.Add(redemption);
                    customer.CurrentCycleWashes -= 7;

                    isFreeWashAwarded = true;
                    awardedRewardId = reward.Id;
                    awardedRewardName = reward.Name;
                    awardedExpiryDate = expiryDate;
                }
                else
                {
                    // Hệ thống không còn quà rửa xe miễn phí nào đang hoạt động.
                    // CỐ Ý KHÔNG trừ CurrentCycleWashes: giữ nguyên quyền lợi để khách được trao
                    // ngay khi vận hành bật lại quà. Log mức Error để đội vận hành phát hiện —
                    // trước đây trường hợp này bị bỏ qua hoàn toàn, không log, khách kẹt vĩnh viễn.
                    _logger.LogError(
                        "Khách {CustomerId} đã đủ 7 lượt rửa nhưng không có phần thưởng rửa xe miễn phí nào đang hoạt động — chưa trao được quà.",
                        customer.Id);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        if (isUpgraded && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
        {
            await SendUpgradeEmailSafeAsync(customer.User.Email, oldTierName, newTierName, point.TotalPoints);
        }

        if (isFreeWashAwarded && awardedRewardId.HasValue && awardedExpiryDate.HasValue && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
        {
            string expireStr = awardedExpiryDate.Value.ToString("dd/MM/yyyy HH:mm");
            await _notificationService.SendNotificationToCustomerAsync(
                customer.Id,
                "Quà tặng tri ân độc quyền! 🎉",
                $"Hoàn thành 7 lượt dịch vụ, bạn được tặng: {awardedRewardName}. Hạn sử dụng: {expireStr}. Đặt lịch ngay!",
                awardedRewardId.Value,
                "Loyalty"
            );
            await SendRewardEmailSafeAsync(customer.User.Email, awardedRewardName, awardedExpiryDate.Value);
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

        var deduct = Math.Min(penalty, available);
        point.AvailablePoints -= deduct;
        point.UpdatedAt = now;

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

        // Tên hàm là "Safe" nhưng trước đây KHÔNG bắt lỗi: SMTP chưa cấu hình là ném thẳng
        // ra ngoài, khiến checkout trả 500 dù điểm/hạng đã commit xong. Email chỉ là thông
        // báo phụ — hỏng thì ghi log, không được làm hỏng giao dịch chính.
        try
        {
            await _emailService.SendEmailAsync(toEmail, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không gửi được email thăng hạng tới {Email}.", toEmail);
        }
    }

    private async Task SendRewardEmailSafeAsync(string toEmail, string rewardName, DateTime expiryDate)
    {
        string subject = "🎁 Bạn có một thẻ quà tặng mới từ hệ thống!";
        string body = $@"
        <h2>Chào bạn,</h2>
        <p>Chúc mừng bạn đã hoàn thành xuất sắc chu kỳ 7 lượt dịch vụ rửa xe.</p>
        <p>Để tri ân, hệ thống đã gửi tặng bạn 1 Voucher: <b>{rewardName}</b>.</p>
        <p>Mã quà tặng này sẽ hết hạn vào lúc: <b style='color:red;'>{expiryDate:dd/MM/yyyy HH:mm}</b>.</p>
        <p>Vui lòng đăng nhập vào ứng dụng và đặt lịch trước thời hạn để không bỏ lỡ phần quà này nhé!</p>
        <br/>
        <p>Cảm ơn bạn đã tin tưởng và đồng hành cùng chúng tôi!</p>
    ";

        // Xem ghi chú ở SendUpgradeEmailSafeAsync: lỗi gửi mail không được làm hỏng checkout.
        try
        {
            await _emailService.SendEmailAsync(toEmail, subject, body); // Gửi qua MailKit đã được cấu hình
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không gửi được email quà tặng tới {Email}.", toEmail);
        }
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
            // TotalPoints giữ = AvailablePoints để FE cũ không vỡ; FE mới đọc AvailablePoints.
            TotalPoints = point?.AvailablePoints ?? 0,
            AvailablePoints = point?.AvailablePoints ?? 0,
            LifetimePoints = point?.TotalPoints ?? 0,
            TotalWashes = customer.TotalWashes,
            CurrentCycleWashes = customer.CurrentCycleWashes,
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

    // Quy tắc hạng DÙNG CHUNG (checkout + worker nền):
    // hạng CAO NHẤT thỏa cả điểm lũy kế (Min) lẫn số booking gần đây (MaintenanceBookings).
    private static Tier? PickTier(List<Tier> tiersDesc, int lifetimePoints, int recentBookings)
        => tiersDesc.FirstOrDefault(t => lifetimePoints >= t.MinPointsRequired
                                      && recentBookings >= t.MaintenanceBookings)
           ?? tiersDesc.LastOrDefault();

    /// <summary>
    /// Rà toàn bộ khách và cập nhật hạng theo SỐ BOOKING hoàn tất trong ~30 ngày gần nhất.
    /// Dùng cho worker nền: HẠ hạng cả khách KHÔNG có booking nào trong kỳ (không cần checkout).
    /// </summary>
    public async Task ReevaluateAllTiersAsync(CancellationToken cancellationToken = default)
    {
        var since = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));

        var tiersDesc = await _context.Tiers
            .OrderByDescending(t => t.MinPointsRequired)
            .ToListAsync(cancellationToken);
        if (tiersDesc.Count == 0) return;

        // Đếm booking hoàn tất 30 ngày cho TẤT CẢ khách trong 1 query.
        var bookingCounts = await _context.Bookings
            .Where(b => b.BookingDate >= since
                     && (b.Status == BookingStatus.Completed || b.Status == BookingStatus.CheckedOut)
                     // Lượt rửa miễn phí không tính để giữ hạng (khớp với AwardPointsForBookingAsync).
                     && (b.Reward == null || !b.Reward.IsFreeWash))
            .GroupBy(b => b.CustomerId)
            .Select(g => new { CustomerId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        var countByCustomer = bookingCounts.ToDictionary(x => x.CustomerId, x => x.Count);

        var pointsByUser = await _context.Points
            .ToDictionaryAsync(p => p.UserId, p => p.TotalPoints, cancellationToken);

        var customers = await _context.Customers.ToListAsync(cancellationToken);
        int changed = 0;
        foreach (var customer in customers)
        {
            var lifetime = pointsByUser.TryGetValue(customer.UserId, out var tp) ? tp : 0;
            var recent = countByCustomer.TryGetValue(customer.Id, out var c) ? c : 0;
            var target = PickTier(tiersDesc, lifetime, recent);
            if (target == null || target.Id == customer.TierId) continue;

            // tiersDesc giảm dần theo MinPointsRequired => index LỚN hơn = hạng THẤP hơn.
            var currentIndex = tiersDesc.FindIndex(t => t.Id == customer.TierId);
            var targetIndex = tiersDesc.FindIndex(t => t.Id == target.Id);

            if (currentIndex >= 0 && targetIndex > currentIndex)
            {
                // HẠ hạng: chỉ lùi ĐÚNG 1 bậc mỗi lần worker chạy, dù đủ điều kiện rớt sâu hơn.
                // Lần sweep sau (mỗi 6h) sẽ tiếp tục lùi tiếp 1 bậc nếu vẫn không đủ số booking.
                customer.TierId = tiersDesc[currentIndex + 1].Id;
                changed++;
            }
            else
            {
                // Nâng/khôi phục hạng (hoặc không xác định được hạng hiện tại): áp thẳng hạng đủ điều kiện.
                customer.TierId = target.Id;
                changed++;
            }
        }

        if (changed > 0)
            await _context.SaveChangesAsync(cancellationToken);
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
