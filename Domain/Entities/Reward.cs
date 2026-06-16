namespace Domain.Entities;

public class Reward
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>How many loyalty points a customer must spend to redeem this reward.</summary>
    public int PointsCost { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<RewardRedemption> Redemptions { get; set; } = new List<RewardRedemption>();
}
