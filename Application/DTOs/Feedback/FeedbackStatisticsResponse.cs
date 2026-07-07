namespace Application.DTOs.Feedback
{
    public class FeedbackStatisticsResponse
    {
        public List<StaffRatingResponse> TopStaffs { get; set; } = new();
        public List<StaffRatingResponse> LowestStaffs { get; set; } = new();
        public List<ServiceRatingResponse> TopServices { get; set; } = new();
        public List<ServiceRatingResponse> LowestServices { get; set; } = new();
        public List<StaffRatingResponse> TopChatStaffs { get; set; } = new();
        public List<StaffRatingResponse> LowestChatStaffs { get; set; } = new();
    }
}
