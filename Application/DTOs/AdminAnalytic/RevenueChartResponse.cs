namespace Application.DTOs.AdminAnalytic
{
    public class RevenueChartResponse
    {
        public string Label { get; set; } = null!; // Ví dụ: "Tháng 01", "Quý 1", "Năm 2025"
        public decimal CurrentPeriodRevenue { get; set; }
        public decimal PreviousPeriodRevenue { get; set; }
        public decimal DifferenceAmount { get; set; }
        public double GrowthPercentage { get; set; }
        public List<BranchRevenueResponse> BranchRevenues { get; set; } = new();
    }
}
