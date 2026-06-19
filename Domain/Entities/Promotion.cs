using Domain.Enums;

namespace Domain.Entities;

public class Promotion
{
    public Guid Id { get; set; }

    /// <summary>The code a customer enters at booking time. Unique, case-insensitive in practice.</summary>
    public string Code { get; set; } = null!;

    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    /// <summary>Total redemptions allowed across all customers. Null = unlimited.</summary>
    public int? MaxUses { get; set; }
    public int UsedCount { get; set; }

    /// <summary>Minimum order subtotal required to apply this promotion. Null = no minimum.</summary>
    public decimal? MinSpend { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
