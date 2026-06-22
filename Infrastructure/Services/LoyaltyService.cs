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

    public LoyaltyService(IApplicationDbContext context, IOptions<LoyaltyOptions> options)
    {
        _context = context;
        _options = options.Value;
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
        var alreadyEarned = await _context.PointHistories
            .AnyAsync(h => h.BookingId == bookingId && h.TransactionType == LoyaltyTransactionType.Earn, cancellationToken);
        if (alreadyEarned)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.Id == booking.CustomerId, cancellationToken)
            ?? throw new AppException("Customer profile not found.", 404);

        var points = (int)Math.Floor(booking.TotalPrice * _options.PointsPerCurrencyUnit * customer.Tier.PointRate);
        var now = DateTime.UtcNow;

        var point = await GetOrCreatePointAsync(customer.UserId, now, cancellationToken);

        // Update the point balance (spendable + lifetime) and the customer's CRM stats.
        point.AvailablePoints += points;
        point.TotalPoints += points;
        point.UpdatedAt = now;
        customer.TotalWashes += 1;
        customer.TotalSpent += booking.TotalPrice;

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
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
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
