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
    public DateTime CreatedAt { get; set; }
}
