using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/time-slots")]
public class TimeSlotController : ControllerBase
{
    private readonly ITimeSlotService _timeSlotService; // Đổi tên biến cho đúng nghiệp vụ

    public TimeSlotController(ITimeSlotService timeSlotService)
    {
        _timeSlotService = timeSlotService;
    }
    
    /// <summary>
    /// API lấy danh sách tổng hợp lịch trống 7 ngày liên tiếp khớp chuẩn UI dạng lưới
    /// </summary>
    [HttpGet("weekly-summary")]
    [AllowAnonymous]
    public async Task<IActionResult> GetWeeklySummary([FromQuery] Guid branchId, [FromQuery] DateOnly? startDate)
    {
        // Nếu Frontend không truyền ngày bắt đầu, tự lấy ngày hôm nay
        var targetStartDate = startDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var weeklySummary = await _timeSlotService.GetWeeklySlotsSummaryAsync(branchId, targetStartDate);
        return Ok(weeklySummary);
    }
}