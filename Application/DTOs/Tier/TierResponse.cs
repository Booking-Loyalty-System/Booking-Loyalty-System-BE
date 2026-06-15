namespace Application.DTOs.Tier;

public class TierResponse
{
    public Guid Id { get; set; }
    public string TierName { get; set; } = string.Empty;
    public decimal PointRate { get; set; }
    public int BookingWindow { get; set; }
    public string Level { get; set; } = string.Empty;
}