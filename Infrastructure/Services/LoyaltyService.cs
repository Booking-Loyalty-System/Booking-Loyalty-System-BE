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
        var alreadyEarned = await _context.LoyaltyTransactions
            .AnyAsync(lt => lt.BookingId == bookingId && lt.Type == LoyaltyTransactionType.Earn, cancellationToken);
        if (alreadyEarned)
        {
            await transaction.CommitAsync(cancellationToken);
            return;
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == booking.CustomerId, cancellationToken)
            ?? throw new AppException("Customer profile not found.", 404);

        var points = (int)Math.Floor(booking.TotalPrice * _options.PointsPerCurrencyUnit);
        var now = DateTime.UtcNow;

        // Update the customer's running CRM totals (points, lifetime, visits, spend).
        customer.TotalPoints += points;
        customer.LifetimePoints += points;
        customer.TotalWashes += 1;
        customer.TotalSpent += booking.TotalPrice;

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
    }

    public async Task<LoyaltyBalanceResponse> GetBalanceAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        return new LoyaltyBalanceResponse
        {
            TotalPoints = customer.TotalPoints,
            LifetimePoints = customer.LifetimePoints,
            TotalWashes = customer.TotalWashes,
            TotalSpent = customer.TotalSpent,
            Tier = customer.Tier.ToString()
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
