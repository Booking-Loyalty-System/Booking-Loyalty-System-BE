using Application.DTOs.Feedback;

namespace Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> CustomerCreateFeedbackAsync(Guid customerId, FeedbackRequest request);
        /* Task<FeedbackModerationResponse> StaffReplyFeedbackAsync(Guid staffId, Guid feedbackId, ReplyFeedbackRequest request);*/
        Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync();
    }
}
