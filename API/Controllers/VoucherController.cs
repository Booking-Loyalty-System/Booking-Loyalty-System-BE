using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/vouchers")]
[Authorize]
public class VoucherController : ControllerBase
{
    private readonly IVoucherService _voucherService;

    public VoucherController(IVoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableVouchers()
    {
        var result = await _voucherService.GetAvailableVouchersAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost("{id:guid}/redeem")]
    public async Task<IActionResult> Redeem(Guid id)
    {
        var result = await _voucherService.RedeemVoucherAsync(GetUserId(), id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Voucher redeemed successfully."));
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyVouchers()
    {
        var result = await _voucherService.GetMyVouchersAsync(GetUserId());
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("applicable")]
    public async Task<IActionResult> GetApplicableVouchers([FromQuery] decimal subtotal)
    {
        var result = await _voucherService.GetApplicableVouchersAsync(GetUserId(), subtotal);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
