using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/statistics")]
[Authorize(Roles = "Staff,Admin")]
public class AdminStatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public AdminStatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        var result = await _statisticsService.GetOverviewAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] string groupBy = "day")
    {
        var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var result = await _statisticsService.GetRevenueAsync(fromDate, toDate, groupBy);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("bookings")]
    public async Task<IActionResult> GetBookingStats(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] string groupBy = "day")
    {
        var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var result = await _statisticsService.GetBookingStatsAsync(fromDate, toDate, groupBy);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopCustomers([FromQuery] int top = 10)
    {
        var result = await _statisticsService.GetTopCustomersAsync(top);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("branch-performance")]
    public async Task<IActionResult> GetBranchPerformance(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var fromDate = from ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var result = await _statisticsService.GetBranchPerformanceAsync(fromDate, toDate);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("tier-distribution")]
    public async Task<IActionResult> GetTierDistribution()
    {
        var result = await _statisticsService.GetTierDistributionAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
