namespace Application.DTOs.Feedback
{
    public class CustomerFeedbackResponse
    {
        public string CustomerName { get; set; }
        public double OverallRating { get; set; }
        public int StaffRating { get; set; }
        public int ServiceRating { get; set; }
        public int PriceRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class BranchFeedbackSummaryResponse
    {
        public double AverageRating { get; set; } // Điểm trung bình cộng (VD: 4.8)
        public int TotalFeedbacks { get; set; }  // Tổng số lượt đánh giá
        public IEnumerable<CustomerFeedbackResponse> Feedbacks { get; set; } // Danh sách feedback
    }
}
