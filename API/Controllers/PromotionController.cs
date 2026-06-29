using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/promotions")]
[Authorize]
public class PromotionController : ControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    /// <summary>
    /// Lists promotions the signed-in customer can actually use right now
    /// (active, in-window, not exhausted, đúng hạng + đúng tháng sinh nhật).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var result = await _promotionService.GetActiveAsync(GetUserId());
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// Previews the discount a code would give for a subtotal, before booking. Enforces the
    /// customer's eligibility (sinh nhật / hạng / chi nhánh). Pass branchId to check chi nhánh.
    /// </summary>
    [HttpGet("{code}/preview")]
    public async Task<IActionResult> Preview(string code, [FromQuery] decimal subtotal, [FromQuery] Guid? branchId = null)
    {
        var result = await _promotionService.PreviewAsync(code, subtotal, GetUserId(), branchId);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
