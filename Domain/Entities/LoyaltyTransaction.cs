using Domain.Enums;

namespace Domain.Entities;

public class LoyaltyTransaction
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public LoyaltyTransactionType Type { get; set; }

    /// <summary>Signed point delta: positive for Earn/Adjust-up, negative for Redeem/Expire.</summary>
    public int Points { get; set; }

    /// <summary>Customer's running TotalPoints immediately after this transaction was applied.</summary>
    public int BalanceAfter { get; set; }

    /// <summary>The booking that produced these points, when applicable.</summary>
    public Guid? BookingId { get; set; }

    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>When earned points expire (Earn rows only). Null for non-expiring rows.</summary>
    public DateTime? ExpiresAt { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Booking? Booking { get; set; }
}
