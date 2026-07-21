namespace Application.DTOs.Feedback
{
    public class ServiceRatingResponse
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
