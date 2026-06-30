namespace Application.DTOs.Tier;

public class UpdateTierRequest
{
    public string? TierName { get; set; }
    public decimal? PointRate { get; set; }
    public int? BookingWindow { get; set; }
    public string? Level { get; set; }
    public int? MinPointsRequired { get; set; }
    public int? MaintenancePoints { get; set; }
}
