namespace Application.DTOs.Feedback
{
    public class FeedbackResponse
    {
        public string BookingCode { get; set; }
        public string CustomerName { get; set; }
        public int StaffRating { get; set; }
        public int ServiceRating { get; set; }
        public int PriceRating { get; set; }
        public double OverallRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
