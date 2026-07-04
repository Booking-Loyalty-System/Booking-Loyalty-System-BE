using Application.DTOs.Feedback;

namespace Application.Interfaces
{
    public interface IAIService
    {
        Task<(string sessionId, string response)> ChatWithCustomerAsync(Guid userId, string customerMessage);
        Task<FeedbackModerationResponse> ModerateFeedbackAsync(string feedbackComment);
    }
}
