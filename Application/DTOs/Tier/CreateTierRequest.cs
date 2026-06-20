namespace Application.DTOs.Tier;

public class CreateTierRequest
{
    public string TierName { get; set; } = null!;
    public decimal PointRate { get; set; }
    public int BookingWindow { get; set; }
    public string Level { get; set; } = null!;
    public int MinPointsRequired { get; set; }
    public int MaintenancePoints { get; set; }
}
