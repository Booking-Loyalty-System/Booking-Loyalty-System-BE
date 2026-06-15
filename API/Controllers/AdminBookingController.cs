using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/bookings")]
[Authorize(Roles = "Staff")]
public class AdminBookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public AdminBookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await _bookingService.CompleteBookingAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Booking completed and loyalty points awarded."));
    }
}
