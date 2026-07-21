using Application.DTOs.ChatMessage;

namespace Application.DTOs.ChatFeedback
{
    public class ChatFeedbackDetailResponse
    {
        public Guid FeedbackId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ChatSessionId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? StaffId { get; set; }
        public string StaffName { get; set; }
        public List<ChatMessageResponse> Messages { get; set; } = new();
    }
}
