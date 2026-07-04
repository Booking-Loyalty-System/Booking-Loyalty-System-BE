using Application.DTOs.ChatFeedback;

namespace Application.Interfaces
{
    public interface IChatFeedbackService
    {
        Task<bool> CreateChatFeedbackAsync(CreateChatFeedbackRequest request);
    }
}
