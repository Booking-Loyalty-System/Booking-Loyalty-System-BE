using Application.DTOs.Admin;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AdminUserService : IAdminUserService
{
    private readonly IApplicationDbContext _context;

    public AdminUserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AdminUserResponse>> GetUsersAsync(string? roleFilter)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(roleFilter) && Enum.TryParse<UserRole>(roleFilter, true, out var role))
        {
            query = query.Where(u => u.Role == role);
        }

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        var result = new List<AdminUserResponse>();

        foreach (var user in users)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            result.Add(new AdminUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                FullName = customer?.FullName,
                PhoneNumber = customer?.PhoneNumber,
                CreatedAt = user.CreatedAt
            });
        }

        return result;
    }

    public async Task UpdateUserStatusAsync(Guid userId, UpdateUserStatusRequest request)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new AppException("User not found.", 404);

        if (user.Role == UserRole.Admin)
            throw new AppException("Cannot change status of admin users.", 400);

        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(Guid userId, UpdateUserRoleRequest request)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new AppException("User not found.", 404);

        if (user.Role == UserRole.Admin)
            throw new AppException("Cannot change role of admin users.", 400);

        if (!Enum.TryParse<UserRole>(request.Role, true, out var newRole))
            throw new AppException("Invalid role.", 400);

        // Only Customer <-> Staff promotion/demotion is allowed; admins are provisioned out-of-band.
        if (newRole == UserRole.Admin)
            throw new AppException("Cannot assign the Admin role.", 400);

        user.Role = newRole;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
