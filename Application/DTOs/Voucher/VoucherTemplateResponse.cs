namespace Application.DTOs.Voucher;

public class VoucherTemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public int PointsCost { get; set; }
    public decimal? MinSpend { get; set; }
}
