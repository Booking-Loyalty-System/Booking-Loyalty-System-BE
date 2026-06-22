using Application.DTOs.Staff;
using Application.Interfaces;
using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
[Route("api/staff")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [Authorize(Roles = "Staff")]
    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<StaffProfileResponse>>> GetMyProfile()
    {
        // Lấy NameIdentifier (UserId) được mã hóa trong JWT Token khi đăng nhập thành công
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(ApiResponse<object>.FailResponse("Người dùng chưa được xác thực hoặc Token không hợp lệ."));
        }

        var profile = await _staffService.GetProfileByUserIdAsync(userId);
        
        return Ok(ApiResponse<StaffProfileResponse>.SuccessResponse(profile, "Lấy thông tin nhân viên thành công."));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<StaffProfileResponse>>> GetStaffById(Guid id)
    {
        var staff = await _staffService.GetStaffByIdAsync(id);
        
        return Ok(ApiResponse<StaffProfileResponse>.SuccessResponse(staff, $"Lấy thông tin nhân viên có ID {id} thành công."));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StaffProfileResponse>>> CreateStaff([FromBody] CreateStaffRequest request)
    {
        var newStaff = await _staffService.CreateStaffAsync(request);
        
        return Ok(ApiResponse<StaffProfileResponse>.SuccessResponse(newStaff, "Tạo tài khoản nhân viên thành công."));
    }
}