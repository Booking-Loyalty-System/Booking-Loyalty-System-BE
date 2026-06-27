using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/rewards")]
[Authorize]
public class RewardController : ControllerBase
{
    private readonly IRewardService _rewardService;

    public RewardController(IRewardService rewardService)
    {
        _rewardService = rewardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _rewardService.GetAllAsync(activeOnly: true);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost("{id:guid}/redeem")]
    public async Task<IActionResult> Redeem(Guid id)
    {
        var result = await _rewardService.RedeemAsync(GetUserId(), id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Reward redeemed successfully."));
    }

    [HttpGet("me/redemptions")]
    public async Task<IActionResult> GetMyRedemptions()
    {
        var result = await _rewardService.GetMyRedemptionsAsync(GetUserId());
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
