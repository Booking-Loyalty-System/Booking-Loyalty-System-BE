using Application.Common;
using Application.DTOs.WashBay;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/wash-bays")]
[Authorize(Roles = "Staff,Admin")]
public class AdminWashBayController : ControllerBase
{
    private readonly IWashBayService _washBayService;

    public AdminWashBayController(IWashBayService washBayService)
    {
        _washBayService = washBayService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _washBayService.GetAllAsync();
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _washBayService.GetByIdAsync(id);
        if (result == null)
            return NotFound(ApiResponse<object>.FailResponse("Wash bay not found."));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWashBayRequest request)
    {
        var result = await _washBayService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.SuccessResponse(result, "Wash bay created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWashBayRequest request)
    {
        var result = await _washBayService.UpdateAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Wash bay updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _washBayService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Wash bay deleted successfully."));
    }
}
