namespace Application.DTOs.Chat
{
    public class ChatSessionResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid? StaffId { get; set; }
        public string? StaffName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ChatMessageResponse> Messages { get; set; } = new();
    }
}
