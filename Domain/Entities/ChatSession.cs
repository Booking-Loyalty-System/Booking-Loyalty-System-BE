namespace Domain.Entities
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? StaffId { get; set; }

        public Customer Customer { get; set; }
        public Staff? Staff { get; set; }
        public ChatSessionStatus Status { get; set; } = ChatSessionStatus.WithAI;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }

    public enum ChatSessionStatus
    {
        WithAI,
        WaitingForStaff,
        HandledByStaff,
        Closed
    }
}
