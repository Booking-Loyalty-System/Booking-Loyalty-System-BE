using Application.Common;
using Application.DTOs.Promotion;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/promotions")]
[Authorize(Roles = "Staff")]
public class AdminPromotionController : ControllerBase
{
    private readonly IPromotionService _promotionService;

    public AdminPromotionController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _promotionService.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _promotionService.GetByIdAsync(id);
        if (result == null)
            return NotFound(ApiResponse<object>.FailResponse("Promotion not found."));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
    {
        var result = await _promotionService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.SuccessResponse(result, "Promotion created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePromotionRequest request)
    {
        var result = await _promotionService.UpdateAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Promotion updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _promotionService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Promotion deleted successfully."));
    }
}
