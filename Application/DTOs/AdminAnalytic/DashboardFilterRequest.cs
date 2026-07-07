namespace Application.DTOs.AdminAnalytic
{
    public class DashboardFilterRequest
    {
        public string Type { get; set; }
        public int Year { get; set; }
        public int? Value { get; set; }
    }
}
