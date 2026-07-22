using Application.DTOs.Feedback;

namespace Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> CustomerCreateFeedbackAsync(Guid customerId, FeedbackRequest request);
        Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync();
        Task<List<FeedbackFilterResponse>> GetFeedbacksAsync(
     string? sortBy = "newest", // newest, oldest, lowest-rating, highest-rating
     bool? isGiftedFilter = null);
        Task<FeedbackStatisticsResponse> GetFeedbackStatisticsAsync(int topCount = 5);
        Task<BranchFeedbackSummaryResponse> GetCustomerFeedbacksAsync(Guid? branchId = null, int pageIndex = 1, int pageSize = 10);
    }
}
