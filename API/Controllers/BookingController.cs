using Application.Common;
using Application.DTOs.Booking;
using Application.Interfaces;
using Domain.Enums;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBookingRequest request)
    {
        var userId = GetUserId();
        var result = await _bookingService.UpdateBookingAsync(userId, id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking updated successfully."));
    }

    [Authorize]
    [HttpGet("{id:guid}/download-invoice")]
    public async Task<IActionResult> DownloadInvoice(Guid id)
    {
        try
        {
            byte[] pdfBytes = await _bookingService.GenerateInvoiceBytesAsync(id);

            // 2. Lấy lại mã đơn hàng để đặt tên file (Hoặc bạn có thể lấy tên tùy ý)
            // Để tránh query lại, ta có thể đặt tên file động dựa trên ID luôn
            string fileName = $"Invoice_{id.ToString().Substring(0, 8).ToUpper()}.pdf";

            // 3. Trả file về trình duyệt trực tiếp
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.FailResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.FailResponse("Có lỗi xảy ra trong quá trình xuất hóa đơn: " + ex.Message));
        }
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
