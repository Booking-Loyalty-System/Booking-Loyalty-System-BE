namespace Application.DTOs.Voucher;

public class CustomerVoucherResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public decimal? MinSpend { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsUsed { get; set; }
}
