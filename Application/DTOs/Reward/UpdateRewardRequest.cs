namespace Application.DTOs.Reward;

public class UpdateRewardRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PointsCost { get; set; }
    public decimal? DiscountAmount { get; set; }
    public bool? IsActive { get; set; }
}
