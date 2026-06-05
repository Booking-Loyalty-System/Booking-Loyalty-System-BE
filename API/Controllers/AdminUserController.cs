using Application.Common;
using Application.DTOs.Admin;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUserController : ControllerBase
{
    private readonly IAdminUserService _adminUserService;

    public AdminUserController(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? role)
    {
        var result = await _adminUserService.GetUsersAsync(role);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusRequest request)
    {
        await _adminUserService.UpdateUserStatusAsync(id, request);
        return Ok(ApiResponse<object>.SuccessResponse(null, "User status updated successfully."));
    }
}
