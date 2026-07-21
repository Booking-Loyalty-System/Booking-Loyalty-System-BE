namespace Application.DTOs.Statistics;

public class BookingStatsResponse
{
    public List<RevenueDataPoint> DataPoints { get; set; } = new();
    public List<BookingStatusSummary> StatusSummary { get; set; } = new();
}

public class BookingStatusSummary
{
    public string Status { get; set; } = null!;
    public int Count { get; set; }
}
