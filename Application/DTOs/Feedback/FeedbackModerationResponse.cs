namespace Application.DTOs.Feedback
{
    public class FeedbackModerationResponse
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; } = null!;
        public string CleanedComment { get; set; } = null!;
    }
}
