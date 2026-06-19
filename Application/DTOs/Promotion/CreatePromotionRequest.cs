namespace Application.DTOs.Promotion;

public class CreatePromotionRequest
{
    public string Code { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>"Percentage" or "FixedAmount".</summary>
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? MaxUses { get; set; }
    public decimal? MinSpend { get; set; }
}
