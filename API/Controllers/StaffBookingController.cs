using Application.Common;
using Application.DTOs.Staff;
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

    [HttpGet("today")]
    public async Task<IActionResult> GetTodayBookings([FromQuery] Guid branchId)
    {
        var result = await _staffBookingService.GetTodayBookingsAsync(branchId);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Today's bookings retrieved successfully."));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByCode([FromQuery] string code)
    {
        var result = await _staffBookingService.SearchByBookingCodeAsync(code);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking found."));
    }

    [HttpPut("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        var result = await _staffBookingService.ConfirmBookingAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking confirmed successfully."));
    }

    [HttpPut("{id:guid}/start")]
    public async Task<IActionResult> StartWash(Guid id)
    {
        var result = await _staffBookingService.StartWashAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Wash started successfully."));
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> CompleteWash(Guid id)
    {
        var result = await _staffBookingService.CompleteWashAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Wash completed successfully. Loyalty points awarded."));
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelBookingByStaffRequest? request)
    {
        var result = await _staffBookingService.CancelBookingAsync(id, request?.Reason);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking cancelled successfully."));
    }
}
