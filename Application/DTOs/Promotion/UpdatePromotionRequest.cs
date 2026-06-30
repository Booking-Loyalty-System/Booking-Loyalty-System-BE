namespace Application.DTOs.Promotion;

public class UpdatePromotionRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public int? PriorityLevel { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxUses { get; set; }
    public decimal? MinSpend { get; set; }
    public bool? IsActive { get; set; }
    public bool? RequiresBirthday { get; set; }

    /// <summary>Khi != null sẽ thay thế toàn bộ danh sách hạng áp dụng (rỗng = gỡ giới hạn hạng).</summary>
    public List<Guid>? TierIds { get; set; }

    /// <summary>Khi != null sẽ thay thế toàn bộ danh sách chi nhánh áp dụng (rỗng = gỡ giới hạn chi nhánh).</summary>
    public List<Guid>? BranchIds { get; set; }
}
