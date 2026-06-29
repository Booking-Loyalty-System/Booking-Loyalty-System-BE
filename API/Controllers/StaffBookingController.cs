using System.Globalization;
using System.Security.Claims;
using Application.Common;
using Application.DTOs.Booking;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/staff/bookings")]
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
    [Authorize(Roles = "Staff")] // 🌟 Chỉ có nhân viên mới được xem danh sách lịch đặt xe
    public async Task<IActionResult> GetByDate([FromQuery] string? date)
    {
        var userId = GetUserId();
        DateOnly target;
        if (string.IsNullOrWhiteSpace(date))
            target = DateOnly.FromDateTime(DateTime.UtcNow);
        else if (!DateOnly.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out target))
            return BadRequest(ApiResponse<object>.FailResponse("Invalid date. Expected format YYYY-MM-DD."));

        var result = await _staffBookingService.GetByDateAsync(userId, target);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    // --- CÁC ENDPOINT CẬP NHẬT TRẠNG THÁI ---

    [HttpPatch("{id:guid}/confirm")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        var result = await _staffBookingService.ConfirmAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã xác nhận lịch đặt."));
    }

    [HttpPatch("{id:guid}/check-in")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> CheckIn(Guid id, Guid staffId)
    {
        var result = await _staffBookingService.CheckInAsync(id, staffId);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã check-in và gán nhân viên."));
    }

    [HttpPatch("{id:guid}/queue")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> Queue(Guid id, Guid bayId)
    {
        var result = await _staffBookingService.QueueAsync(id, bayId);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã đưa xe vào khoang chờ."));
    }

    [HttpPatch("{id:guid}/start")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> Start(Guid id, Guid? bayId)
    {
        var result = await _staffBookingService.StartServiceAsync(id, bayId);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã bắt đầu thực hiện dịch vụ."));
    }

    [HttpPatch("{id:guid}/checkout")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> CheckOut(Guid id)
    {
        var result = await _staffBookingService.CheckOutAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã hoàn thành và check-out."));
    }

    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Roles = "Staff,Customer")] // 🌟 Cho phép Staff HOẶC Customer gọi API hủy
    public async Task<IActionResult> Cancel(Guid id, string cancel)
    {
        var result = await _staffBookingService.CancelAsync(id, cancel);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã hủy lịch đặt."));
    }

    [HttpPatch("{id:guid}/no-show")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> NoShow(Guid id)
    {
        var result = await _staffBookingService.NoShowAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Đã đánh dấu khách không đến."));
    }

    [HttpPatch("{id:guid}/completed")]
    [Authorize(Roles = "Staff,Customer")]
    public async Task<IActionResult> Completed(Guid id)
    {
        var result = await _staffBookingService.CompleteServiceAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Hoàn thành dịch vụ."));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
