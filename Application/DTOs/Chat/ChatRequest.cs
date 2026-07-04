namespace Application.DTOs.Chat
{
    public class ChatRequest
    {
        public string Message { get; set; } = null!;
        public string? HistoryContext { get; set; }
    }
}
