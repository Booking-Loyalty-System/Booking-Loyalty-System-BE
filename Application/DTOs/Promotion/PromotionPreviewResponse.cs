namespace Application.DTOs.Promotion;

public class PromotionPreviewResponse
{
    public string Code { get; set; } = null!;
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
}
