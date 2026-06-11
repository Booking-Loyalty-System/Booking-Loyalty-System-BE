using Application.Common;
using Application.DTOs.WashPackage;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/wash-packages")]
[Authorize(Roles = "Staff")]
public class AdminWashPackageController : ControllerBase
{
    private readonly IWashPackageService _washPackageService;

    public AdminWashPackageController(IWashPackageService washPackageService)
    {
        _washPackageService = washPackageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _washPackageService.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _washPackageService.GetByIdAsync(id);
        if (result == null)
            return NotFound(ApiResponse<object>.FailResponse("Wash package not found."));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWashPackageRequest request,
        [FromServices] IValidator<CreateWashPackageRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _washPackageService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.SuccessResponse(result, "Wash package created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWashPackageRequest request)
    {
        var result = await _washPackageService.UpdateAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Wash package updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _washPackageService.DeactivateAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Wash package deactivated successfully."));
    }
}
