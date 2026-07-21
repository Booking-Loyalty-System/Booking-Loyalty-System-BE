namespace Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid ChatSessionId { get; set; }
        public ChatSession ChatSession { get; set; } = null!;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string SenderType { get; set; } = null!;

        public Guid? StaffId { get; set; }
        public Staff? Staff { get; set; }

        public string Message { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
