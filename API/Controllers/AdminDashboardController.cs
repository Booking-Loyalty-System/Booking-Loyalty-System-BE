using Application.Common;
using Application.DTOs.AdminDashboard;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(ApiResponse<DashboardSummaryResponse>.SuccessResponse(result));
        }

        [HttpGet("recent-bookings")]
        public async Task<IActionResult> GetRecentBookings()
        {
            var result = await _dashboardService.GetRecentBookingsAsync(5);
            return Ok(ApiResponse<List<RecentBookingResponse>>.SuccessResponse(result));
        }

        [HttpGet("tier-config")]
        public async Task<IActionResult> GetTierConfig()
        {
            var result = await _dashboardService.GetTierConfigAsync();
            return Ok(ApiResponse<TierConfigDto>.SuccessResponse(result));
        }

        [HttpPut("tier-config")]
        public async Task<IActionResult> UpdateTierConfig([FromBody] TierConfigDto request)
        {
            await _dashboardService.UpdateTierConfigAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(null, "Tier configuration saved successfully."));
        }

        [HttpGet("export-rbl")]
        public async Task<IActionResult> ExportRblDataset()
        {
            var fileBytes = await _dashboardService.ExportRBLDatasetAsync();
            var fileName = $"RBL_Dataset_{DateTime.UtcNow:yyyy-MM-dd}.csv";

            // Trả thẳng file về cho client tải xuống
            return File(fileBytes, "text/csv", fileName);
        }

        [HttpGet("top-customers")]
        public async Task<IActionResult> GetTopCustomers([FromQuery] string timeFrame = "month")
        {
            var result = await _dashboardService.GetTopCustomersAsync(timeFrame, 10);
            return Ok(ApiResponse<List<TopCustomerDto>>.SuccessResponse(result));
        }

        [HttpGet("top-branches")]
        public async Task<IActionResult> GetTopBranches([FromQuery] string timeFrame = "month")
        {
            var result = await _dashboardService.GetTopBranchesAsync(timeFrame);
            return Ok(ApiResponse<List<TopBranchDto>>.SuccessResponse(result));
        }

        [HttpGet("top-timeslots")]
        public async Task<IActionResult> GetTopTimeSlots([FromQuery] string timeFrame = "month")
        {
            var result = await _dashboardService.GetTopTimeSlotsAsync(timeFrame, 10);
            return Ok(ApiResponse<List<TopTimeSlotDto>>.SuccessResponse(result));
        }
    }
}
