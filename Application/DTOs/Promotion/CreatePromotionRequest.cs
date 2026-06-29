namespace Application.DTOs.Promotion;

public class CreatePromotionRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>"Percentage" or "FixedAmount".</summary>
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public int PriorityLevel { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? MaxUses { get; set; }
    public decimal? MinSpend { get; set; }

    /// <summary>True = chỉ dùng được trong tuần sinh nhật của khách (sinh nhật ± 3 ngày).</summary>
    public bool RequiresBirthday { get; set; }

    /// <summary>Giới hạn theo hạng: rỗng/null = mọi hạng; có phần tử = chỉ các hạng này.</summary>
    public List<Guid>? TierIds { get; set; }

    /// <summary>Giới hạn theo chi nhánh (địa chỉ): rỗng/null = mọi chi nhánh; có phần tử = chỉ các chi nhánh này.</summary>
    public List<Guid>? BranchIds { get; set; }
}
