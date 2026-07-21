namespace Application.DTOs.Reward;

public class RedemptionResponse
{
    public Guid Id { get; set; }
    public Guid RewardId { get; set; }
    public string RewardName { get; set; } = null!;
    public int PointsSpent { get; set; }
    public string Status { get; set; } = null!;

    /// <summary>The customer's loyalty-point balance immediately after this redemption.</summary>
    public int BalanceAfter { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
}
