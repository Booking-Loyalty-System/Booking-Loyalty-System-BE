namespace Application.DTOs.Reward;

public class RewardResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int PointsCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
