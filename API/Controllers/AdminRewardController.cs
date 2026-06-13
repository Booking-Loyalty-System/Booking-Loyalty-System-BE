using Application.Common;
using Application.DTOs.Reward;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/rewards")]
[Authorize(Roles = "Staff")]
public class AdminRewardController : ControllerBase
{
    private readonly IRewardService _rewardService;

    public AdminRewardController(IRewardService rewardService)
    {
        _rewardService = rewardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _rewardService.GetAllAsync(activeOnly: false);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _rewardService.GetByIdAsync(id);
        if (result == null)
            return NotFound(ApiResponse<object>.FailResponse("Reward not found."));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRewardRequest request)
    {
        var result = await _rewardService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<object>.SuccessResponse(result, "Reward created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRewardRequest request)
    {
        var result = await _rewardService.UpdateAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Reward updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _rewardService.DeleteAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Reward deleted successfully."));
    }

    [HttpPost("redemptions/{id:guid}/fulfill")]
    public async Task<IActionResult> Fulfill(Guid id)
    {
        var result = await _rewardService.FulfillAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Redemption fulfilled."));
    }
}
