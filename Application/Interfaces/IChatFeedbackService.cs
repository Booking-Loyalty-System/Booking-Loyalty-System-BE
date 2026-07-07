using Application.DTOs.ChatFeedback;

namespace Application.Interfaces
{
    public interface IChatFeedbackService
    {
        Task<bool> CreateChatFeedbackAsync(CreateChatFeedbackRequest request);
        Task<IEnumerable<ChatFeedbackResponse>> GetLatestChatFeedbacksAsync(int count = 10);
        Task<ChatStaffStatisticResponse> GetTopChatStaffAsync(int topCount = 5);
        Task<ChatFeedbackDetailResponse> GetChatFeedbackDetailAsync(Guid feedbackId);
    }
}
