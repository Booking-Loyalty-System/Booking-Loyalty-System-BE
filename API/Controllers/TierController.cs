using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/tiers")]
public class TierController : ControllerBase
{
    private readonly ITierService _tierService;

    public TierController(ITierService tierService)
    {
        _tierService = tierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _tierService.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyTier()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _tierService.GetMyTierAsync(userId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
