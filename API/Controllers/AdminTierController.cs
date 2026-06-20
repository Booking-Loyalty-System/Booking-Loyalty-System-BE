using Application.Common;
using Application.DTOs.Tier;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/tiers")]
[Authorize(Roles = "Staff,Admin")]
public class AdminTierController : ControllerBase
{
    private readonly ITierService _tierService;

    public AdminTierController(ITierService tierService)
    {
        _tierService = tierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _tierService.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _tierService.GetByIdAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTierRequest request,
        [FromServices] IValidator<CreateTierRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _tierService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<object>.SuccessResponse(result, "Tier created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTierRequest request,
        [FromServices] IValidator<UpdateTierRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _tierService.UpdateAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Tier updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _tierService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Tier deleted successfully."));
    }
}
