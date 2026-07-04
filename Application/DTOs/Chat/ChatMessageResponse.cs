namespace Application.DTOs.Chat
{
    public class ChatMessageResponse
    {
        public Guid Id { get; set; }
        public string SenderType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
