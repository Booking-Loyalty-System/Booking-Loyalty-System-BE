using Application.Common;
using Application.DTOs.Payment;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/payments")]
[Authorize(Roles = "Staff,Admin")]
public class AdminPaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public AdminPaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("audit")]
    public async Task<IActionResult> Audit([FromQuery] PaymentAuditRequest request)
    {
        var result = await _paymentService.GetPaymentsAuditAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
