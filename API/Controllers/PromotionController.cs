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

    /// <summary>Lists promotions currently available to customers (active, in-window, not exhausted).</summary>
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var result = await _promotionService.GetActiveAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>Previews the discount a code would give for a subtotal, before booking.</summary>
    [HttpGet("{code}/preview")]
    public async Task<IActionResult> Preview(string code, [FromQuery] decimal subtotal)
    {
        var result = await _promotionService.PreviewAsync(code, subtotal);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
