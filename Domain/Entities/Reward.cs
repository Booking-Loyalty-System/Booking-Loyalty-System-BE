namespace Domain.Entities;

public class Reward
{
    public Guid Id { get; set; }
    public int PointsRequired { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool Status { get; set; }

    public ICollection<Booking> Bookings = new List<Booking>();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>How many loyalty points a customer must spend to redeem this reward.</summary>
    public int PointsCost { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Human-readable voucher code shown to the customer (e.g. "VOUCHER_100K", "FREEWASH_BASIC").</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>True when redeeming grants a complimentary wash instead of a money discount.</summary>
    public bool IsFreeWash { get; set; }

    /// <summary>For a free-wash voucher, the wash package it can be redeemed against. Null otherwise.</summary>
    public Guid? WashPackageId { get; set; }

    // Navigation
    public ICollection<RewardRedemption> Redemptions { get; set; } = new List<RewardRedemption>();
    public WashPackage? WashPackage { get; set; }
}
