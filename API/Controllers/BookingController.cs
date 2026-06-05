using System.Security.Claims;
using Application.Common;
using Application.DTOs.Booking;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request,
        [FromServices] IValidator<CreateBookingRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var userId = GetUserId();
        var result = await _bookingService.CreateBookingAsync(userId, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking created successfully."));
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserId();
        var result = await _bookingService.GetBookingByIdAsync(userId, id);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [Authorize]
    [HttpGet("my-bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = GetUserId();
        var result = await _bookingService.GetMyBookingsAsync(userId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [Authorize]
    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelBookingRequest? request)
    {
        var userId = GetUserId();
        var result = await _bookingService.CancelBookingAsync(userId, id, request?.Reason);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking cancelled successfully."));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}

public class CancelBookingRequest
{
    public string? Reason { get; set; }
}
