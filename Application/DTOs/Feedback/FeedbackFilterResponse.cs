namespace Application.DTOs.Feedback
{
    public class FeedbackFilterResponse
    {
        public Guid Id { get; set; }
        public string BookingCode { get; set; } = null!;
        public double OverallRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
