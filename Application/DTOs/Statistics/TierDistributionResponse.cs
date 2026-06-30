namespace Application.DTOs.Statistics;

public class TierDistributionResponse
{
    public string TierName { get; set; } = null!;
    public int CustomerCount { get; set; }
    public decimal Percentage { get; set; }
}
