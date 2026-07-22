using Application.DTOs.Statistics;
using Application.DTOs.Tier;

namespace Application.Interfaces;

public interface IStatisticsService
{
    Task<OverviewResponse> GetOverviewAsync();
    Task<List<RevenueDataPoint>> GetRevenueAsync(DateOnly from, DateOnly to, string groupBy);
    Task<BookingStatsResponse> GetBookingStatsAsync(DateOnly from, DateOnly to, string groupBy);
    Task<List<TopCustomerResponse>> GetTopCustomersAsync(int top);
    Task<List<BranchPerformanceResponse>> GetBranchPerformanceAsync(DateOnly from, DateOnly to);
    Task<List<TierDistributionResponse>> GetTierDistributionAsync();
    Task<List<TierPeriodReportResponse>> GetTierStatisticsAsync(TierStatisticsRequest request);
}
