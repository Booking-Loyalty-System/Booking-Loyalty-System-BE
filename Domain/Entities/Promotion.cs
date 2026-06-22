using Domain.Enums;

namespace Domain.Entities;

public class Promotion
{
    public Guid Id { get; set; }

    /// <summary>The code a customer enters at booking time. Unique, case-insensitive in practice.</summary>
    public string Code { get; set; } = null!;
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public DiscountType DiscountType { get; set; }
    
    public decimal DiscountValue { get; set; }

    public int PriorityLevel { get; set; } // Giữ lại trường này từ file Configuration cũ của bạn

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    /// <summary>Total redemptions allowed across all customers. Null = unlimited.</summary>
    public int? MaxUses { get; set; }
    
    public int UsedCount { get; set; }

    /// <summary>Minimum order subtotal required to apply this promotion. Null = no minimum.</summary>
    public decimal? MinSpend { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>True = this promotion is a voucher redeemable with loyalty points.</summary>
    public bool IsVoucher { get; set; } = false;

    /// <summary>Loyalty points required to redeem this voucher. Only relevant when IsVoucher = true.</summary>
    public int? PointsCost { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Quan hệ 1-Nhiều với PromotionBranch (Cần thiết để Configuration map được)
    public ICollection<PromotionBranch> PromotionBranches { get; set; } = new List<PromotionBranch>();
}