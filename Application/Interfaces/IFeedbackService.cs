using Application.DTOs.Feedback;

namespace Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> CustomerCreateFeedbackAsync(Guid customerId, FeedbackRequest request);
        Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync();
        Task<List<FeedbackFilterResponse>> GetFeedbacksAsync(bool isDescending = true);
        Task<FeedbackStatisticsResponse> GetFeedbackStatisticsAsync(int topCount = 5);
    }
}
