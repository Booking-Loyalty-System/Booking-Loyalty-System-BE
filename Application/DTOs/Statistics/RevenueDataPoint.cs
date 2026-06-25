namespace Application.DTOs.Statistics;

public class RevenueDataPoint
{
    public string Date { get; set; } = null!;
    public decimal Revenue { get; set; }
    public int BookingCount { get; set; }
}
