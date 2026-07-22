namespace Application.DTOs.Tier
{
    public class TierCountResponse
    {
        public Guid TierId { get; set; }
        public string TierName { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
        public double PercentageChangeFromPrevious { get; set; }
    }
}
