namespace Application.DTOs.Feedback
{
    public class FeedbackFilterResponse
    {
        public Guid Id { get; set; }
        public string BookingCode { get; set; } = null!;
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public int StaffRating { get; set; }
        public int ServiceRating { get; set; }
        public int PriceRating { get; set; }
        public bool IsGifted { get; set; }
        public double OverallRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
