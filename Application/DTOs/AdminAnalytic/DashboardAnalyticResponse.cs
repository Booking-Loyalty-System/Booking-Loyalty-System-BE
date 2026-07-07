namespace Application.DTOs.AdminAnalytic
{
    public class DashboardAnalyticResponse
    {
        public decimal TotalRevenue { get; set; }
        public List<RevenueChartResponse> MonthlyRevenue { get; set; } = new();
        public List<RevenueChartResponse> QuarterlyRevenue { get; set; } = new();
        public List<RevenueChartResponse> YearlyRevenue { get; set; } = new();
    }
}
