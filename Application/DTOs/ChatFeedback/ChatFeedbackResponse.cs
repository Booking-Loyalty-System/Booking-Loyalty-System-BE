namespace Application.DTOs.ChatFeedback
{
    public class ChatFeedbackResponse
    {
        public Guid Id { get; set; }
        public Guid ChatSessionId { get; set; }
        public string CustomerName { get; set; }
        public string StaffName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
