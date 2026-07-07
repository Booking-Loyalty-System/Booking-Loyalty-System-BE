namespace Application.DTOs.Feedback
{
    public class StaffRatingResponse
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; } = null!;
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
