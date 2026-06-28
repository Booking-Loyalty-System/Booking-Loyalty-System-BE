using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/tiers")]
[Authorize]
public class TierController : ControllerBase
{
    private readonly ITierService _tierService;

    public TierController(ITierService tierService)
    {
        _tierService = tierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTiers()
    {
        var result = await _tierService.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTierById(Guid id)
    {
        var result = await _tierService.GetByIdAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyTier()
    {
        var userId = GetUserId();
        var result = await _tierService.GetMyTierAsync(userId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
