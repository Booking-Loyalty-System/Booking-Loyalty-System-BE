using Application.Common;
using Application.DTOs.AddOn;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/add-ons")]
[Authorize(Roles = "Staff,Admin")]
public class AdminAddOnController : ControllerBase
{
    private readonly IAddOnService _addOnService;

    public AdminAddOnController(IAddOnService addOnService)
    {
        _addOnService = addOnService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _addOnService.GetAllAsync(activeOnly: false);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _addOnService.GetByIdAsync(id);
        if (result == null)
            return NotFound(ApiResponse<object>.FailResponse("Add-on not found."));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAddOnRequest request)
    {
        var result = await _addOnService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.SuccessResponse(result, "Add-on created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAddOnRequest request)
    {
        var result = await _addOnService.UpdateAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Add-on updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _addOnService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Add-on deleted successfully."));
    }
}
