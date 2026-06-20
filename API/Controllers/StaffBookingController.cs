using System.Globalization;
using Application.Common;
using Application.DTOs.Booking;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/staff/bookings")]
[Authorize(Roles = "Staff,Admin")]
public class StaffBookingController : ControllerBase
{
    private readonly IStaffBookingService _staffBookingService;

    public StaffBookingController(IStaffBookingService staffBookingService)
    {
        _staffBookingService = staffBookingService;
    }

    [HttpGet("scan-qr")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> ScanQrCode([FromQuery] string payload)
    {
        var result = await _staffBookingService.GetBookingByQrPayloadAsync(payload);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Quét mã QR thành công."));
    }
    
    /// <summary>Lists the bookings for a given day. Defaults to today when no date is supplied.</summary>
    [HttpGet]
    [Authorize(Roles = "Staff")] // 🌟 THÊM Ở ĐÂY: Chỉ có nhân viên mới được xem danh sách lịch đặt xe
    public async Task<IActionResult> GetByDate([FromQuery] string? date)
    {
        DateOnly target;
        if (string.IsNullOrWhiteSpace(date))
            target = DateOnly.FromDateTime(DateTime.UtcNow);
        else if (!DateOnly.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out target))
            return BadRequest(ApiResponse<object>.FailResponse("Invalid date. Expected format YYYY-MM-DD."));

        var result = await _staffBookingService.GetByDateAsync(target);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Staff,Customer")] // 🌟 THÊM Ở ĐÂY: Cho phép Staff HOẶC Customer gọi API này
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateBookingStatusRequest request)
    {
        var result = await _staffBookingService.UpdateStatusAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, $"Booking status updated to {request.TargetStatus}."));
    }
}