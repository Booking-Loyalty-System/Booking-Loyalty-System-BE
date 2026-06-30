namespace Application.DTOs.Reward;

/// <summary>
/// A customer's personal voucher (a redeemed reward), shaped for the loyalty FE contract.
/// </summary>
public class VoucherResponse
{
    /// <summary>The redemption id (the voucher instance owned by the customer).</summary>
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int RequiredPoints { get; set; }
    public string? Description { get; set; }

    /// <summary>Money discount in VND. 0 for a free-wash voucher.</summary>
    public decimal DiscountValue { get; set; }

    /// <summary>"Active" | "Used" | "Expired".</summary>
    public string Status { get; set; } = null!;
    public DateTime? ExpiryDate { get; set; }
    public bool IsFreeWash { get; set; }
    public Guid? WashPackageId { get; set; }
}
