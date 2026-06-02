using Application.DTOs.Admin;

namespace Application.Interfaces;

public interface IAdminUserService
{
    Task<List<AdminUserResponse>> GetUsersAsync(string? roleFilter);
    Task UpdateUserStatusAsync(Guid userId, UpdateUserStatusRequest request);
}
