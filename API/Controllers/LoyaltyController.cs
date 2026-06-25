using System.Security.Claims;
using Application.Common;
using Application.DTOs.Reward;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/loyalty")]
[Authorize]
public class LoyaltyController : ControllerBase
{
    private readonly ILoyaltyService _loyaltyService;
    private readonly IRewardService _rewardService;

    public LoyaltyController(ILoyaltyService loyaltyService, IRewardService rewardService)
    {
        _loyaltyService = loyaltyService;
        _rewardService = rewardService;
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var userId = GetUserId();
        var result = await _loyaltyService.GetBalanceAsync(userId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userId = GetUserId();
        var result = await _loyaltyService.GetHistoryAsync(userId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// The current user's personal vouchers (redeemed rewards). Active first, then by expiry.
    /// Pass <c>?activeOnly=true</c> to get only still-usable vouchers (hides Used/Expired).
    /// </summary>
    [HttpGet("my-vouchers")]
    public async Task<IActionResult> GetMyVouchers([FromQuery] bool activeOnly = false)
    {
        var result = await _rewardService.GetMyVouchersAsync(GetUserId(), activeOnly);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>Spend points to redeem a reward; returns the newly created voucher.</summary>
    [HttpPost("redeem")]
    public async Task<IActionResult> Redeem([FromBody] RedeemVoucherRequest request)
    {
        var result = await _rewardService.RedeemVoucherAsync(GetUserId(), request.RewardId);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Voucher redeemed successfully"));
    }

    /// <summary>Mark one of the current user's vouchers as used.</summary>
    [HttpPost("vouchers/{id:guid}/use")]
    public async Task<IActionResult> UseVoucher(Guid id)
    {
        await _rewardService.UseVoucherAsync(GetUserId(), id);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "Voucher marked as used"));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
