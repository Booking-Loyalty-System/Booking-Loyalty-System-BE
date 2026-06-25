namespace Application.DTOs.Statistics;

public class OverviewResponse
{
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
    public int TotalCustomers { get; set; }
    public int CompletedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public int NoShowBookings { get; set; }
    public int NewCustomersThisMonth { get; set; }
    public decimal AvgRevenuePerBooking { get; set; }
}
