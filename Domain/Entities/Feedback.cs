namespace Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }

        public Booking Booking { get; set; }
        public Customer Customer { get; set; }

        public int StaffRating { get; set; }
        public int ServiceRating { get; set; }
        public int PriceRating { get; set; }
        public double OverallRating => (StaffRating + ServiceRating + PriceRating) / 3.0;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
