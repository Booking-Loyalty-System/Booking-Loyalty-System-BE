using Application.Common;
using Application.DTOs.Tier;
using Application.Interfaces;
using FluentValidation;
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
        var result = await _tierService.GetAllTiersAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTierById(Guid id)
    {
        var result = await _tierService.GetTierByIdAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTier(
        [FromBody] CreateTierRequest request,
        [FromServices] IValidator<CreateTierRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _tierService.CreateTierAsync(request);
        
        // Trả về mã 201 Created cùng với location để lấy chi tiết Tier vừa tạo
        return CreatedAtAction(nameof(GetTierById), new { id = result.Id }, ApiResponse<object>.SuccessResponse(result, "Tier created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTier(
        Guid id,
        [FromBody] UpdateTierRequest request,
        [FromServices] IValidator<UpdateTierRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _tierService.UpdateTierAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Tier updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTier(Guid id)
    {
        await _tierService.DeleteTierAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Tier deleted successfully."));
    }
}