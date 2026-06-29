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
    }
}
