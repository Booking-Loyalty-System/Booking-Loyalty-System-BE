namespace Application.DTOs.Promotion;

public class UpdatePromotionRequest
{
    public string? Description { get; set; }
    public string? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxUses { get; set; }
    public decimal? MinSpend { get; set; }
    public bool? IsActive { get; set; }
}
