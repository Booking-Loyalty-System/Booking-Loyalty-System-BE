using Application.DTOs.Staff;

namespace Application.Interfaces;

public interface IStaffService
{
    Task<StaffProfileResponse> GetProfileByUserIdAsync(Guid userId);
    Task<StaffProfileResponse> CreateStaffAsync(CreateStaffRequest request);
    Task<StaffProfileResponse> GetStaffByIdAsync(Guid staffId);
    Task<List<StaffProfileResponse>> GetAllStaffAsync(StaffFilterRequest filter);
    Task<StaffProfileResponse> UpdateStaffAsync(Guid staffId, UpdateStaffRequest request);
    Task DeleteStaffAsync(Guid staffId);
}