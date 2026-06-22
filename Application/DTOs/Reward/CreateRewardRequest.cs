namespace Application.DTOs.Reward;

public class CreateRewardRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int PointsCost { get; set; }

    /// <summary>Fixed-amount discount the voucher grants when applied to a booking.</summary>
    public decimal DiscountAmount { get; set; }
}
