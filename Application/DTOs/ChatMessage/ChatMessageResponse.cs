namespace Application.DTOs.ChatMessage
{
    public class ChatMessageResponse
    {
        public Guid Id { get; set; }
        public string SenderType { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
