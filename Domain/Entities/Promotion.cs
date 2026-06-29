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

    /// <summary>
    /// Khi true, khuyến mãi này chỉ dùng được trong THÁNG SINH NHẬT của khách (theo Customer.DateOfBirth).
    /// Điều kiện hạng áp qua <see cref="TierPromotions"/>, điều kiện chi nhánh/địa chỉ qua <see cref="PromotionBranches"/>.
    /// </summary>
    public bool RequiresBirthday { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PromotionBranch> PromotionBranches { get; set; } = new List<PromotionBranch>();
    public ICollection<TierPromotion> TierPromotions { get; set; } = new List<TierPromotion>();
}