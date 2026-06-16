using Domain.Enums;

namespace Domain.Entities;

public class RewardRedemption
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RewardId { get; set; }

    /// <summary>Points debited for this redemption (snapshot of the reward's PointsCost).</summary>
    public int PointsSpent { get; set; }

    public RedemptionStatus Status { get; set; } = RedemptionStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Set when staff hands the reward to the customer in-store.</summary>
    public DateTime? FulfilledAt { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Reward Reward { get; set; } = null!;
}
