namespace Application.DTOs.Promotion;

public class PromotionResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? MaxUses { get; set; }
    public int UsedCount { get; set; }
    public decimal? MinSpend { get; set; }
    public bool IsActive { get; set; }

    /// <summary>True = chỉ dùng được trong tuần sinh nhật của khách (sinh nhật ± 3 ngày).</summary>
    public bool RequiresBirthday { get; set; }

    /// <summary>Hạng áp dụng: rỗng = mọi hạng; có phần tử = chỉ các hạng này.</summary>
    public List<Guid> EligibleTierIds { get; set; } = new();

    /// <summary>Chi nhánh (địa chỉ) áp dụng: rỗng = mọi chi nhánh; có phần tử = chỉ các chi nhánh này.</summary>
    public List<Guid> EligibleBranchIds { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}
