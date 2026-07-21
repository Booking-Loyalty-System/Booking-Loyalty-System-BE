using Application.DTOs.AdminDashboard;

namespace Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardSummaryResponse> GetDashboardSummaryAsync();
        Task<List<RecentBookingResponse>> GetRecentBookingsAsync(int limit = 5);
        Task<TierConfigDto> GetTierConfigAsync();
        Task UpdateTierConfigAsync(TierConfigDto request);
        Task<byte[]> ExportRBLDatasetAsync();
        Task<List<TopCustomerDto>> GetTopCustomersAsync(string timeFrame, int limit = 10);
        Task<List<TopBranchDto>> GetTopBranchesAsync(string timeFrame);
        Task<List<TopTimeSlotDto>> GetTopTimeSlotsAsync(string timeFrame, int limit = 5);

        // New admin dashboard APIs
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<List<RevenueByDateDto>> GetDailyRevenueAsync(int month, int year);
        Task<List<PackageAnalyticsDto>> GetPackageAnalyticsAsync(int top = 10);
        Task<FeedbackSummaryDto> GetFeedbackSummaryAsync();
        Task<RevenueComparisonDto> GetRevenueComparisonAsync(AdminDashboardDateFilterDto filter);
    }
}
