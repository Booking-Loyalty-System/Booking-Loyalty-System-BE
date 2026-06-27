using Application.DTOs.Staff;

namespace Application.Interfaces;

public interface IStaffService
{
    Task<StaffProfileResponse> GetProfileByUserIdAsync(Guid userId);
    Task<StaffProfileResponse> CreateStaffAsync(CreateStaffRequest request);
    Task<StaffProfileResponse> GetStaffByIdAsync(Guid staffId);
}