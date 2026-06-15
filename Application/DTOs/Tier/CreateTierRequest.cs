namespace Application.DTOs.Tier;

public class CreateTierRequest
{
    public string TierName { get; set; } = string.Empty;
    public decimal PointRate { get; set; }
    public int BookingWindow { get; set; }
    public string Level { get; set; } = string.Empty;
}