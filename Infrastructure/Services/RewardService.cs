using Application.DTOs.Reward;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RewardService : IRewardService
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;
    public RewardService(
    ApplicationDbContext context,
    INotificationService notificationService,
    IEmailService emailService)
    {
        _context = context;
        _notificationService = notificationService;
        _emailService = emailService;
    }

    // ----- Catalog management -----

    public async Task<List<RewardResponse>> GetAllAsync(bool activeOnly, bool includeFreeWash = true)
    {
        var query = _context.Rewards.AsQueryable();
        if (activeOnly)
            query = query.Where(r => r.IsActive);

        // Danh mục đổi điểm của khách không được liệt kê quà rửa xe miễn phí:
        // loại quà này chỉ trao tự động sau 7 lượt rửa, không đổi bằng điểm.
        // Admin vẫn xem được đầy đủ để quản lý.
        if (!includeFreeWash)
            query = query.Where(r => !r.IsFreeWash);

        var rewards = await query
            .OrderBy(r => r.PointsCost)
            .ToListAsync();

        return rewards.Select(MapToResponse).ToList();
    }

    public async Task<RewardResponse?> GetByIdAsync(Guid id)
    {
        var reward = await _context.Rewards.FindAsync(id);
        return reward == null ? null : MapToResponse(reward);
    }

    public async Task<RewardResponse> CreateAsync(CreateRewardRequest request)
    {
        var reward = new Reward
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            PointsCost = request.PointsCost,
            DiscountAmount = request.DiscountAmount,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            WashPackageId = request.WashPackageId,
            IsFreeWash = request.WashPackageId.HasValue
        };

        _context.Rewards.Add(reward);
        await _context.SaveChangesAsync();

        return MapToResponse(reward);
    }

    public async Task<RewardResponse> UpdateAsync(Guid id, UpdateRewardRequest request)
    {
        var reward = await _context.Rewards.FindAsync(id)
            ?? throw new AppException("Reward not found.", 404);

        if (request.Name != null) reward.Name = request.Name;
        if (request.Description != null) reward.Description = request.Description;
        if (request.PointsCost.HasValue) reward.PointsCost = request.PointsCost.Value;
        if (request.DiscountAmount.HasValue) reward.DiscountAmount = request.DiscountAmount.Value;
        if (request.IsActive.HasValue) reward.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();

        return MapToResponse(reward);
    }

    public async Task DeleteAsync(Guid id)
    {
        var reward = await _context.Rewards.FindAsync(id)
            ?? throw new AppException("Reward not found.", 404);

        _context.Rewards.Remove(reward);
        await _context.SaveChangesAsync();
    }

    // ----- Redemption -----

    public async Task<RedemptionResponse> RedeemAsync(Guid userId, Guid rewardId)
    {
        var (redemption, reward, balanceAfter) = await RedeemCoreAsync(userId, rewardId);
        return MapRedemption(redemption, reward.Name, balanceAfter);
    }

    public async Task<VoucherResponse> RedeemVoucherAsync(Guid userId, Guid rewardId)
    {
        var (redemption, reward, _) = await RedeemCoreAsync(userId, rewardId);
        return MapVoucher(redemption, reward);
    }

    private async Task<(RewardRedemption redemption, Reward reward, int balanceAfter)> RedeemCoreAsync(Guid userId, Guid rewardId)
    {
        // Serializable so two concurrent redemptions cannot both read the same balance
        // and overspend the customer's points.
        await using var transaction = await _context.BeginTransactionAsync();

        var reward = await _context.Rewards
            .FirstOrDefaultAsync(r => r.Id == rewardId)
            ?? throw new AppException("Reward not found.", 404);

        if (!reward.IsActive)
            throw new AppException("Reward is not available.", 400);

        // Voucher rửa xe miễn phí là quà tri ân, chỉ trao tự động sau khi khách hoàn thành
        // đủ 7 lượt rửa TRẢ TIỀN. Trước đây chúng có PointsCost = 0 mà vẫn nằm trong danh mục
        // đổi điểm, nên khách đổi được không giới hạn chỉ với 1 lượt rửa trong chu kỳ.
        if (reward.IsFreeWash)
            throw new AppException(
                "Phần thưởng rửa xe miễn phí chỉ được trao sau khi bạn hoàn thành đủ 7 lượt rửa, không thể đổi bằng điểm.", 400);

        // Trước đây chỉ kiểm IsActive nên phần thưởng đã hết hạn vẫn đổi được bình thường.
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (today < reward.StartDate || today > reward.EndDate)
            throw new AppException("Phần thưởng này không còn trong thời gian áp dụng.", 400);

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var now = DateTime.UtcNow;

        // Khách chưa từng tích điểm thì chưa có dòng Points; tạo mới thay vì báo "không đủ điểm"
        // (thông báo sai bản chất khi phần thưởng có giá 0 điểm).
        var point = await _context.Points
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (point is null)
        {
            point = new Point
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AvailablePoints = 0,
                TotalPoints = 0,
                UpdatedAt = now
            };
            _context.Points.Add(point);
        }

        if (point.AvailablePoints < reward.PointsCost)
            throw new AppException("Insufficient points to redeem this reward.", 400);

        point.AvailablePoints -= reward.PointsCost;
        point.UpdatedAt = now;

        // Redeeming a reward spends points and produces a voucher: a Redeem ledger row
        // carrying the RewardId and an expiry date (per ERD Point History).
        var ledger = new PointHistory
        {
            Id = Guid.NewGuid(),
            PointId = point.Id,
            TransactionType = LoyaltyTransactionType.Redeem,
            Amount = -reward.PointsCost,
            BalanceAfter = point.AvailablePoints,
            RewardId = reward.Id,
            Description = $"Redeemed {reward.Name}",
            CreatedAt = now,
            ExpiryDate = now.AddDays(30)
        };

        var redemption = new RewardRedemption
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            RewardId = reward.Id,
            PointsSpent = reward.PointsCost,
            Status = RedemptionStatus.Pending,
            CreatedAt = now,
            ExpiryDate = now.AddDays(30)
        };

        _context.PointHistories.Add(ledger);
        _context.RewardRedemptions.Add(redemption);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return (redemption, reward, point.AvailablePoints);
    }

    // ----- Voucher contract (loyalty FE) -----

    public async Task<List<VoucherResponse>> GetMyVouchersAsync(Guid userId, bool activeOnly = false)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var redemptions = await _context.RewardRedemptions
            .Include(rr => rr.Reward)
            .Where(rr => rr.CustomerId == customer.Id)
            .ToListAsync();

        var vouchers = redemptions.Select(rr => MapVoucher(rr, rr.Reward));

        // activeOnly: chỉ trả voucher còn dùng được, để FE không render nhầm voucher đã dùng/hết hạn.
        if (activeOnly)
            vouchers = vouchers.Where(v => v.Status == "Active");

        // Active first, then by soonest expiry.
        return vouchers
            .OrderBy(v => v.Status == "Active" ? 0 : 1)
            .ThenBy(v => v.ExpiryDate ?? DateTime.MaxValue)
            .ToList();
    }

    public async Task UseVoucherAsync(Guid userId, Guid voucherId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var redemption = await _context.RewardRedemptions
            .FirstOrDefaultAsync(rr => rr.Id == voucherId && rr.CustomerId == customer.Id)
            ?? throw new AppException("Voucher not found.", 404);

        if (redemption.Status == RedemptionStatus.Fulfilled)
            throw new AppException("Voucher has already been used.", 400);
        if (redemption.Status == RedemptionStatus.Cancelled)
            throw new AppException("Voucher is no longer valid.", 400);
        if (redemption.ExpiryDate is not null && redemption.ExpiryDate <= DateTime.UtcNow)
            throw new AppException("Voucher has expired.", 400);

        redemption.Status = RedemptionStatus.Fulfilled;
        redemption.FulfilledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<List<RedemptionResponse>> GetMyRedemptionsAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        return await _context.RewardRedemptions
            .Where(rr => rr.CustomerId == customer.Id)
            .OrderByDescending(rr => rr.CreatedAt)
            .Select(rr => new RedemptionResponse
            {
                Id = rr.Id,
                RewardId = rr.RewardId,
                RewardName = rr.Reward.Name,
                PointsSpent = rr.PointsSpent,
                Status = rr.Status.ToString(),
                BalanceAfter = 0,
                CreatedAt = rr.CreatedAt,
                FulfilledAt = rr.FulfilledAt
            })
            .ToListAsync();
    }

    public async Task<List<RedemptionResponse>> GetRedemptionsAsync(string? status = null, int pageIndex = 0, int pageSize = 20)
    {
        var query = _context.RewardRedemptions
            .Include(rr => rr.Reward)
            .Include(rr => rr.Customer)
            .ThenInclude(c => c.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (Enum.TryParse<RedemptionStatus>(status, true, out var st))
                query = query.Where(rr => rr.Status == st);
        }

        var items = await query
            .OrderByDescending(rr => rr.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(rr => new RedemptionResponse
            {
                Id = rr.Id,
                RewardId = rr.RewardId,
                RewardName = rr.Reward.Name,
                PointsSpent = rr.PointsSpent,
                Status = rr.Status.ToString(),
                BalanceAfter = 0,
                CreatedAt = rr.CreatedAt,
                FulfilledAt = rr.FulfilledAt
            })
            .ToListAsync();

        return items;
    }

    public async Task<RedemptionResponse> FulfillAsync(Guid redemptionId)
    {
        var redemption = await _context.RewardRedemptions
            .Include(rr => rr.Reward)
            .FirstOrDefaultAsync(rr => rr.Id == redemptionId)
            ?? throw new AppException("Redemption not found.", 404);

        if (redemption.Status != RedemptionStatus.Pending)
            throw new AppException($"Cannot fulfill a redemption that is already {redemption.Status}.", 400);

        redemption.Status = RedemptionStatus.Fulfilled;
        redemption.FulfilledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapRedemption(redemption, redemption.Reward.Name, null);
    }

    public async Task<VoucherResponse> GiftCompensationVoucherAsync(Guid customerId, Guid rewardId, Guid? bookingId)
    {
        await using var transaction = await _context.BeginTransactionAsync();

        if (bookingId.HasValue)
        {
            bool isAlreadyGifted = await _context.RewardRedemptions
                .AnyAsync(r => r.BookingId == bookingId.Value && r.IsGifted);

            if (isAlreadyGifted)
                throw new AppException("Booking này đã được tặng voucher đền bù trước đó.", 400);
        }

        var reward = await _context.Rewards
            .FirstOrDefaultAsync(r => r.Id == rewardId)
            ?? throw new AppException("Reward/Voucher type not found.", 404);

        var customer = await _context.Customers
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == customerId)
            ?? throw new AppException("Customer profile not found.", 404);

        var point = await _context.Points
            .FirstOrDefaultAsync(p => p.UserId == customer.UserId);

        var now = DateTime.UtcNow;
        var expiryDate = now.AddDays(30);

        if (point is not null)
        {
            string description = bookingId.HasValue
            ? $"[Admin Gift] Đền bù Voucher cho Booking {bookingId.Value}: {reward.Name}"
            : $"[Admin Gift] Đền bù Voucher qua Chat: {reward.Name}";

            var ledger = new PointHistory
            {
                Id = Guid.NewGuid(),
                PointId = point.Id,
                TransactionType = LoyaltyTransactionType.Redeem,
                Amount = 0,
                BalanceAfter = point.AvailablePoints,
                RewardId = reward.Id,
                Description = description,
                CreatedAt = now,
                ExpiryDate = expiryDate
            };
            _context.PointHistories.Add(ledger);
        }

        var redemption = new RewardRedemption
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            RewardId = reward.Id,
            PointsSpent = 0,
            Status = RedemptionStatus.Pending,
            CreatedAt = now,
            IsGifted = true,
            ExpiryDate = expiryDate,
            BookingId = bookingId
        };

        _context.RewardRedemptions.Add(redemption);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var notifyTitle = "Quà Tặng Đền Bù - Voucher Mới!";
        var notifyMessage = bookingId.HasValue
            ? $"Bạn đã được tặng voucher '{reward.Name}' đền bù cho sự cố tại lịch đặt dịch vụ #{bookingId.Value.ToString()[..8].ToUpper()}."
            : $"Bạn đã được tặng voucher '{reward.Name}' từ ban quản trị hệ thống.";

        // 1. Gửi thông báo hệ thống & SignalR Realtime đến App của Khách hàng
        try
        {
            await _notificationService.SendNotificationToCustomerAsync(
                customerId: customer.Id,
                title: notifyTitle,
                message: notifyMessage,
                relatedId: redemption.Id,
                type: "Loyalty"
            );
        }
        catch (Exception)
        {
            // Log lại lỗi gửi notification nếu cần, nhưng không ném Exception 
            // để tránh rollback một voucher đã ghi nhận thành công dưới Database.
        }

        // 2. Gửi Email thông báo trực tiếp đến Hòm thư Khách hàng
        if (customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
        {
            var emailSubject = "Quà Tặng Đền Bù - Bạn Nhận Được Voucher Mới";
            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                <div style='background-color: #ff9f43; color: #ffffff; padding: 20px; text-align: center;'>
                    <h2 style='margin: 0;'>Món Quà Nho Nhỏ Từ Chúng Tôi!</h2>
                </div>
                <div style='padding: 24px; color: #333333;'>
                    <p>Xin chào <strong>{customer.FullName}</strong>,</p>
                    <p>Chúng tôi rất tiếc vì những trải nghiệm chưa thực sự trọn vẹn của bạn vừa qua. Ban quản trị hệ thống xin gửi tặng bạn một chiếc voucher đền bù:</p>
                    
                    <div style='background-color: #fff9f0; border-left: 4px solid #ff9f43; padding: 15px; margin: 20px 0; border-radius: 4px;'>
                        <h3 style='margin: 0 0 10px 0; color: #d35400;'>Thông tin Voucher:</h3>
                        <p style='margin: 5px 0;'><strong>Tên ưu đãi:</strong> {reward.Name}</p>
                        <p style='margin: 5px 0;'><strong>Ngày hết hạn:</strong> {expiryDate.ToLocalTime():dd/MM/yyyy HH:mm}</p>
                        {(bookingId.HasValue ? $"<p style='margin: 5px 0;'><strong>Đền bù cho đơn đặt lịch:</strong> #{bookingId.Value.ToString()[..8].ToUpper()}</p>" : "")}
                    </div>

                    <p>Voucher này đã được gửi trực tiếp vào tài khoản của bạn và sẵn sàng sử dụng cho lần đặt lịch tiếp theo.</p>
                    <p style='color: #777777;'>Chúng tôi rất trân trọng sự thấu hiểu và đồng hành của bạn!</p>
                </div>
                <div style='background-color: #f8f9fa; text-align: center; padding: 12px; color: #777777; font-size: 12px; border-top: 1px solid #e0e0e0;'>
                    Đây là email tự động gửi từ hệ thống chăm sóc khách hàng. Vui lòng không phản hồi lại email này.
                </div>
            </div>";

            try
            {
                await _emailService.SendEmailAsync(customer.User.Email, emailSubject, emailBody);
            }
            catch (Exception)
            {
                return MapVoucher(redemption, reward); // Log lỗi gửi email nếu cần, nhưng không ném Exception để tránh rollback voucher đã ghi nhận thành công.
            }
        }

        return MapVoucher(redemption, reward);
    }

    // ----- Mapping -----

    private static RewardResponse MapToResponse(Reward reward) => new()
    {
        Id = reward.Id,
        Name = reward.Name,
        Description = reward.Description,
        PointsCost = reward.PointsCost,
        DiscountAmount = reward.DiscountAmount,
        IsActive = reward.IsActive,
        CreatedAt = reward.CreatedAt,
        IsFreeWash = reward.IsFreeWash
    };

    /// <summary>Maps our internal redemption status onto the FE's Active/Used/Expired model.</summary>
    private static string MapVoucherStatus(RewardRedemption rr)
    {
        if (rr.Status == RedemptionStatus.Fulfilled) return "Used";
        if (rr.Status == RedemptionStatus.Cancelled) return "Expired";
        // Pending: Active unless past its expiry.
        if (rr.ExpiryDate is not null && rr.ExpiryDate <= DateTime.UtcNow) return "Expired";
        return "Active";
    }

    private static VoucherResponse MapVoucher(RewardRedemption rr, Reward reward) => new()
    {
        Id = rr.Id,
        Code = reward.Code,
        Title = reward.Name,
        RequiredPoints = rr.PointsSpent,
        Description = reward.Description,
        DiscountValue = reward.DiscountAmount,
        Status = MapVoucherStatus(rr),
        ExpiryDate = rr.ExpiryDate,
        IsGifted = rr.IsGifted,
        BookingId = rr.BookingId,
        IsFreeWash = reward.IsFreeWash,
        WashPackageId = reward.WashPackageId
    };

    private static RedemptionResponse MapRedemption(RewardRedemption rr, string rewardName, int? balanceAfter) => new()
    {
        Id = rr.Id,
        RewardId = rr.RewardId,
        RewardName = rewardName,
        PointsSpent = rr.PointsSpent,
        Status = rr.Status.ToString(),
        BalanceAfter = balanceAfter ?? 0,
        CreatedAt = rr.CreatedAt,
        FulfilledAt = rr.FulfilledAt
    };
}
