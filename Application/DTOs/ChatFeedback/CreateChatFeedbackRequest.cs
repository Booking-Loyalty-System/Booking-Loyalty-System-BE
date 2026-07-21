namespace Application.DTOs.ChatFeedback
{
    public class CreateChatFeedbackRequest
    {
        public Guid ChatSessionId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
