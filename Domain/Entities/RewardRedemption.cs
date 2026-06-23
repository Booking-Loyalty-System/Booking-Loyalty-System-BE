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

    /// <summary>Set when the voucher is consumed (e.g. applied to a booking).</summary>
    public DateTime? FulfilledAt { get; set; }

    /// <summary>When this redeemed voucher expires. Null = no expiry.</summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>The booking this voucher was applied to, once consumed.</summary>
    public Guid? BookingId { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Reward Reward { get; set; } = null!;
    public Booking? Booking { get; set; }
}
