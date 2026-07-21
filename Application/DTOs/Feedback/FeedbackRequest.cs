namespace Application.DTOs.Feedback
{
    public class FeedbackRequest
    {
        public Guid BookingId { get; set; }
        public int ServiceRating { get; set; }
        public int StaffRating { get; set; }
        public int PriceRating { get; set; }
        public string Comment { get; set; }
    }
}
