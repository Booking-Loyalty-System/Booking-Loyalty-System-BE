using Application.DTOs.Reward;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RewardService : IRewardService
{
    private readonly IApplicationDbContext _context;

    public RewardService(IApplicationDbContext context)
    {
        _context = context;
    }

    // ----- Catalog management -----

    public async Task<List<RewardResponse>> GetAllAsync(bool activeOnly)
    {
        var query = _context.Rewards.AsQueryable();
        if (activeOnly)
            query = query.Where(r => r.IsActive);

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
            IsActive = true,
            CreatedAt = DateTime.UtcNow
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
        // Serializable so two concurrent redemptions cannot both read the same balance
        // and overspend the customer's points.
        await using var transaction = await _context.BeginTransactionAsync();

        var reward = await _context.Rewards
            .FirstOrDefaultAsync(r => r.Id == rewardId)
            ?? throw new AppException("Reward not found.", 404);

        if (!reward.IsActive)
            throw new AppException("Reward is not available.", 400);

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var point = await _context.Points
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (point is null || point.AvailablePoints < reward.PointsCost)
            throw new AppException("Insufficient points to redeem this reward.", 400);

        var now = DateTime.UtcNow;
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
            CreatedAt = now
        };

        _context.PointHistories.Add(ledger);
        _context.RewardRedemptions.Add(redemption);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return MapRedemption(redemption, reward.Name, point.AvailablePoints);
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

    // ----- Mapping -----

    private static RewardResponse MapToResponse(Reward reward) => new()
    {
        Id = reward.Id,
        Name = reward.Name,
        Description = reward.Description,
        PointsCost = reward.PointsCost,
        IsActive = reward.IsActive,
        CreatedAt = reward.CreatedAt
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
