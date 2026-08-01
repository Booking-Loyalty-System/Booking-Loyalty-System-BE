namespace Application.DTOs.Report
{
    public class Report
    {
    }

    public class RevenueReportRequest
    {
        public DateTime CurrentFromDate { get; set; }
        public DateTime CurrentToDate { get; set; }
        public DateTime? CompareFromDate { get; set; }
        public DateTime? CompareToDate { get; set; }
    }

    public class RevenueReportResponse
    {
        public DateTime CurrentFromDate { get; set; }
        public DateTime CurrentToDate { get; set; }
        public decimal CurrentRevenue { get; set; }

        public DateTime? CompareFromDate { get; set; }
        public DateTime? CompareToDate { get; set; }
        public decimal CompareRevenue { get; set; }

        public decimal Variance => CurrentRevenue - CompareRevenue;

        public double GrowthPercentage
        {
            get
            {
                if (CompareRevenue == 0) return CurrentRevenue > 0 ? 100 : 0;
                return (double)Math.Round(((CurrentRevenue - CompareRevenue) / CompareRevenue) * 100, 2);
            }
        }
    }
}
