namespace Application.DTOs.Tier;

public class TierResponse
{
    public Guid Id { get; set; }
    public string TierName { get; set; } = null!;
    public string Level { get; set; } = null!;
    public decimal PointRate { get; set; }
    public int BookingWindow { get; set; }
    public int MinPointsRequired { get; set; }
    public int MaintenancePoints { get; set; }
    public List<string> Benefits { get; set; } = new();
}
