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

    /// <summary>Lists the bookings for a given day. Defaults to today when no date is supplied.</summary>
    [HttpGet]
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

    [HttpPost("{id:guid}/check-in")]
    public async Task<IActionResult> CheckIn(Guid id)
    {
        var result = await _staffBookingService.CheckInAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Customer checked in."));
    }

    [HttpPost("{id:guid}/queue")]
    public async Task<IActionResult> Queue(Guid id)
    {
        var result = await _staffBookingService.QueueAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking moved to queue."));
    }

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, [FromBody] StartServiceRequest request)
    {
        var result = await _staffBookingService.StartAsync(id, request.StaffId);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Service started."));
    }

    [HttpPost("{id:guid}/finish")]
    public async Task<IActionResult> Finish(Guid id)
    {
        var result = await _staffBookingService.FinishAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Service finished."));
    }

    [HttpPost("{id:guid}/checkout")]
    public async Task<IActionResult> Checkout(Guid id)
    {
        var result = await _staffBookingService.CheckoutAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking checked out and loyalty points awarded."));
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelServiceRequest? request)
    {
        var result = await _staffBookingService.CancelAsync(id, request?.Reason);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking cancelled."));
    }

    [HttpPost("{id:guid}/no-show")]
    public async Task<IActionResult> NoShow(Guid id)
    {
        var result = await _staffBookingService.NoShowAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking marked as no-show."));
    }
}
