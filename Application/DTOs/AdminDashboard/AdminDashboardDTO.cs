using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AdminDashboard;

public class DashboardSummaryResponse
{
    public MetricsDto Metrics { get; set; } = new();
    public List<RevenueDataDto> RevenueChart { get; set; } = new();
    public List<TierDistributionDto> TierDistribution { get; set; } = new();
}

public class MetricsDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
    public int ActiveCustomers { get; set; }
    public decimal AverageOrderValue { get; set; }
}

public class RevenueDataDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class TierDistributionDto
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class RecentBookingResponse
{
    public string Id { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class TierConfigDto
{
    [Range(0, 10)]
    public double MemberMultiplier { get; set; }
    [Range(0, 10)]
    public double SilverMultiplier { get; set; }
    [Range(0, 10)]
    public double GoldMultiplier { get; set; }
    [Range(0, 10)]
    public double PlatinumMultiplier { get; set; }
}

public class TopCustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string TierName { get; set; } = string.Empty;
    public decimal TotalSpent { get; set; }
    public DateTime LastVisit { get; set; }
    public int Points { get; set; }
}

public class TopBranchDto
{
    public string BranchId { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public int TotalBookings { get; set; }
}

public class TopTimeSlotDto
{
    public string TimeSlotId { get; set; } = string.Empty;
    public string TimeSlotDisplay { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
}
