namespace Application.DTOs.AdminAnalytic
{
    public class BranchRevenueResponse
    {
        public Guid BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public decimal Revenue { get; set; }
    }
}
