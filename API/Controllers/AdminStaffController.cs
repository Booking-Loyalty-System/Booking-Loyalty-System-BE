using Application.Common;
using Application.DTOs.Staff;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/staff")]
[Authorize(Roles = "Admin")]
public class AdminStaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public AdminStaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<StaffProfileResponse>>>> GetAll([FromQuery] StaffFilterRequest filter)
    {
        var result = await _staffService.GetAllStaffAsync(filter);
        return Ok(ApiResponse<List<StaffProfileResponse>>.SuccessResponse(result, "Lấy danh sách nhân viên thành công."));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<StaffProfileResponse>>> GetById(Guid id)
    {
        var staff = await _staffService.GetStaffByIdAsync(id);
        return Ok(ApiResponse<StaffProfileResponse>.SuccessResponse(staff, $"Lấy thông tin nhân viên có ID {id} thành công."));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StaffProfileResponse>>> Create([FromBody] CreateStaffRequest request)
    {
        var newStaff = await _staffService.CreateStaffAsync(request);
        return Ok(ApiResponse<StaffProfileResponse>.SuccessResponse(newStaff, "Tạo tài khoản nhân viên thành công."));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<StaffProfileResponse>>> Update(Guid id, [FromBody] UpdateStaffRequest request)
    {
        var updated = await _staffService.UpdateStaffAsync(id, request);
        return Ok(ApiResponse<StaffProfileResponse>.SuccessResponse(updated, "Cập nhật thông tin nhân viên thành công."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        await _staffService.DeleteStaffAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Vô hiệu hóa tài khoản nhân viên thành công."));
    }
}
