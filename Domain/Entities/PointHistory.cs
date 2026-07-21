using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Ledger of point movements (per ERD: Point History). Replaces the old LoyaltyTransaction.
/// Each row links to a Point balance and optionally to the Booking that earned it or the
/// Reward it was redeemed for. ExpiryDate carries the point/voucher lifetime.
/// </summary>
public class PointHistory
{
    public Guid Id { get; set; }
    public Guid PointId { get; set; }

    /// <summary>Signed point delta (positive = earn, negative = redeem/expire).</summary>
    public int Amount { get; set; }

    public LoyaltyTransactionType TransactionType { get; set; }

    /// <summary>Running balance snapshot after this movement.</summary>
    public int BalanceAfter { get; set; }

    public Guid? BookingId { get; set; }

    /// <summary>Set when this movement redeems a reward (voucher). Null otherwise.</summary>
    public Guid? RewardId { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>When these points (or the redeemed voucher) expire. Null = no expiry.</summary>
    public DateTime? ExpiryDate { get; set; }

    // Navigation
    public Point Point { get; set; } = null!;
    public Booking? Booking { get; set; }
    public Reward? Reward { get; set; }
}
