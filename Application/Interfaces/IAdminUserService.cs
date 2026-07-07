using Application.DTOs.Admin;

namespace Application.Interfaces;

public interface IAdminUserService
{
    Task<List<AdminUserResponse>> GetUsersAsync(string? roleFilter);
    Task UpdateUserStatusAsync(Guid userId, UpdateUserStatusRequest request);
    Task UpdateUserRoleAsync(Guid userId, UpdateUserRoleRequest request);

    // New: Toggle user active status convenience method
    Task ToggleUserActiveStatusAsync(Guid userId);
}
