namespace Application.DTOs.Statistics;

public class BranchPerformanceResponse
{
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int CancelledBookings { get; set; }
}
