using Application.DTOs.AdminAnalytic;

namespace Application.Interfaces
{
    public interface IAdminAnalyticService
    {
        Task<DashboardAnalyticResponse> GetRevenueAnalyticsAsync(DashboardFilterRequest filter);
    }
}
