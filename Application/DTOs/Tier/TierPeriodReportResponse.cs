namespace Application.DTOs.Tier
{
    public class TierPeriodReportResponse
    {
        public string PeriodLabel { get; set; }
        public List<TierCountResponse> Tiers { get; set; } = new();
        public int TotalCustomersInPeriod { get; set; }
    }
}
