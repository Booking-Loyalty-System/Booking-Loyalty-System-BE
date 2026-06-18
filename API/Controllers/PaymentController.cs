using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Authorize]
    [HttpPost("bookings/{bookingId:guid}/checkout")]
    public async Task<IActionResult> Checkout(Guid bookingId)
    {
        var userId = GetUserId();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        var result = await _paymentService.CreatePaymentUrlAsync(userId, bookingId, ip);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Payment URL created."));
    }

    // VNPay server-to-server IPN: the source of truth. Returns the JSON VNPay expects.
    [AllowAnonymous]
    [HttpGet("vnpay/ipn")]
    public async Task<IActionResult> Ipn()
    {
        var query = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        var (rspCode, message) = await _paymentService.ProcessIpnAsync(query);
        return Ok(new { RspCode = rspCode, Message = message });
    }

    // VNPay browser return URL: display-only, used by the frontend to show the result.
    [AllowAnonymous]
    [HttpGet("vnpay/return")]
    public async Task<IActionResult> Return()
    {
        var query = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        var result = await _paymentService.ProcessReturnAsync(query);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
