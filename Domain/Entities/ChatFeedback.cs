namespace Domain.Entities
{
    public class ChatFeedback
    {
        public Guid Id { get; set; }

        public Guid ChatSessionId { get; set; }
        public ChatSession ChatSession { get; set; }

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
