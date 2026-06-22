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

    public LoyaltyService(IApplicationDbContext context, IOptions<LoyaltyOptions> options, IEmailService emailService)
    {
        _context = context;
        _options = options.Value;
        _emailService = emailService;
    }

    public async Task AwardPointsForBookingAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        // Serializable so two concurrent completions of the same booking cannot both
        // pass the "already earned?" check and double-award.
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken: cancellationToken);

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null)
            return; // Nothing to award; caller already validated existence in normal flow.

        // Idempotency: one Earn row per booking. If it already exists, this is a no-op.
        var alreadyEarned = await _context.LoyaltyTransactions
            .AnyAsync(lt => lt.BookingId == bookingId && lt.Type == LoyaltyTransactionType.Earn, cancellationToken);
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

        // Update the customer's running CRM totals (points, lifetime, visits, spend).
        customer.TotalPoints += points;
        customer.LifetimePoints += points;
        customer.TotalWashes += 1;
        customer.TotalSpent += booking.TotalPrice;

        var eligibleTier = await _context.Tiers
            .Where(t => t.MinPointsRequired <= customer.TotalPoints)
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
        
        var earn = new LoyaltyTransaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Type = LoyaltyTransactionType.Earn,
            Points = points,
            BalanceAfter = customer.TotalPoints,
            BookingId = booking.Id,
            Description = $"Earned from booking {booking.BookingCode}",
            CreatedAt = now,
            ExpiresAt = now.AddMonths(_options.PointLifetimeMonths)
        };

        _context.LoyaltyTransactions.Add(earn);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        if (isUpgraded && customer.User != null && !string.IsNullOrEmpty(customer.User.Email))
        {
            await SendUpgradeEmailSafeAsync(customer.User.Email, oldTierName, newTierName, customer.TotalPoints);
        }
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

        return new LoyaltyBalanceResponse
        {
            TotalPoints = customer.TotalPoints,
            LifetimePoints = customer.LifetimePoints,
            TotalWashes = customer.TotalWashes,
            TotalSpent = customer.TotalSpent,
            Tier = customer.Tier?.TierName ?? string.Empty
        };
    }

    public async Task<List<LoyaltyTransactionResponse>> GetHistoryAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        return await _context.LoyaltyTransactions
            .Where(lt => lt.CustomerId == customer.Id)
            .OrderByDescending(lt => lt.CreatedAt)
            .Select(lt => new LoyaltyTransactionResponse
            {
                Id = lt.Id,
                Type = lt.Type.ToString(),
                Points = lt.Points,
                BalanceAfter = lt.BalanceAfter,
                BookingId = lt.BookingId,
                Description = lt.Description,
                CreatedAt = lt.CreatedAt,
                ExpiresAt = lt.ExpiresAt
            })
            .ToListAsync();
    }
}
