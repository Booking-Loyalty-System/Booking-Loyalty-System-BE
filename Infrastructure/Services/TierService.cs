using Application.DTOs.Tier;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TierService : ITierService
{
    private readonly IApplicationDbContext _context;

    public TierService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TierResponse>> GetAllAsync()
    {
        var tiers = await _context.Tiers
            .OrderBy(t => t.MinPointsRequired)
            .ToListAsync();

        return tiers.Select(MapToResponse).ToList();
    }

    public async Task<CustomerTierResponse> GetMyTierAsync(Guid userId)
    {
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        // Find next tier
        var nextTier = await _context.Tiers
            .Where(t => t.MinPointsRequired > customer.LifetimePoints)
            .OrderBy(t => t.MinPointsRequired)
            .FirstOrDefaultAsync();

        // Calculate maintenance: sum of Earn points in the last 90 days
        var windowStart = DateTime.UtcNow.AddDays(-90);
        var recentPoints = await _context.LoyaltyTransactions
            .Where(lt => lt.CustomerId == customer.Id
                         && lt.Type == LoyaltyTransactionType.Earn
                         && lt.CreatedAt >= windowStart)
            .SumAsync(lt => lt.Points);

        // Days remaining: how many days until the oldest transaction in the 90-day window falls off
        var oldestInWindow = await _context.LoyaltyTransactions
            .Where(lt => lt.CustomerId == customer.Id
                         && lt.Type == LoyaltyTransactionType.Earn
                         && lt.CreatedAt >= windowStart)
            .OrderBy(lt => lt.CreatedAt)
            .Select(lt => (DateTime?)lt.CreatedAt)
            .FirstOrDefaultAsync();

        var daysRemaining = oldestInWindow.HasValue
            ? Math.Max(0, (int)(90 - (DateTime.UtcNow - oldestInWindow.Value).TotalDays))
            : 90;

        var maintenanceRequired = customer.Tier.MaintenancePoints;

        return new CustomerTierResponse
        {
            CurrentTier = MapToResponse(customer.Tier),
            LifetimePoints = customer.LifetimePoints,
            NextTier = nextTier != null
                ? new NextTierInfo
                {
                    TierName = nextTier.TierName,
                    MinPointsRequired = nextTier.MinPointsRequired,
                    PointsNeeded = nextTier.MinPointsRequired - customer.LifetimePoints,
                    ProgressPercent = Math.Round(
                        (double)customer.LifetimePoints / nextTier.MinPointsRequired * 100, 1)
                }
                : null,
            Maintenance = new MaintenanceInfo
            {
                RequiredPoints = maintenanceRequired,
                RecentPoints = recentPoints,
                DaysRemaining = daysRemaining,
                IsSafe = recentPoints >= maintenanceRequired
            }
        };
    }

    private static TierResponse MapToResponse(Tier tier) => new()
    {
        Id = tier.Id,
        TierName = tier.TierName,
        Level = tier.Level.ToString(),
        PointRate = tier.PointRate,
        BookingWindow = tier.BookingWindow,
        MinPointsRequired = tier.MinPointsRequired,
        MaintenancePoints = tier.MaintenancePoints,
        Benefits = GenerateBenefits(tier)
    };

    private static List<string> GenerateBenefits(Tier tier) =>
    [
        $"Tích {tier.PointRate:0.#}x điểm mỗi 1.000đ",
        $"Đặt trước {tier.BookingWindow} ngày",
        tier.MaintenancePoints > 0
            ? $"Cần {tier.MaintenancePoints} điểm/90 ngày để duy trì"
            : "Không yêu cầu duy trì"
    ];
}
