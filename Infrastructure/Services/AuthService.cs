using Application.Common;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<TokenResponse>> RegisterAsync(RegisterRequest request)
    {
        // Check duplicate email
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new AppException("Email already exists.", 409);

        // Check duplicate phone
        if (await _context.Customers.AnyAsync(c => c.PhoneNumber == request.PhoneNumber))
            throw new AppException("Phone number already exists.", 409);

        // Check duplicate license plate (non-deleted)
        if (await _context.Vehicles.AnyAsync(v => v.LicensePlate == request.LicensePlate && !v.IsDeleted))
            throw new AppException("License plate already exists.", 409);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Customer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            Tier = CustomerTier.Member,
            CreatedAt = DateTime.UtcNow
        };

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            LicensePlate = request.LicensePlate,
            Type = request.VehicleType,
            IsPrimary = true,
            CreatedAt = DateTime.UtcNow
        };

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        _context.Users.Add(user);
        _context.Customers.Add(customer);
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return ApiResponse<TokenResponse>.SuccessResponse(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60)
        }, "Registration successful.");
    }

    public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AppException("Invalid email or password.", 401);

        if (!user.IsActive)
            throw new AppException("Account is deactivated.", 403);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<TokenResponse>.SuccessResponse(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60)
        }, "Login successful.");
    }

    public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new AppException("Invalid or expired refresh token.", 401);

        // Rotation: generate new pair
        var accessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<TokenResponse>.SuccessResponse(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60)
        }, "Token refreshed successfully.");
    }

    public async Task<ApiResponse<object>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new AppException("User not found.", 404);

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new AppException("Current password is incorrect.", 400);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<object>.SuccessResponse(null!, "Password changed successfully. Please login again.");
    }

    public async Task<ApiResponse<object>> LogoutAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new AppException("User not found.", 404);

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<object>.SuccessResponse(null!, "Logged out successfully.");
    }

    public async Task<ApiResponse<MeResponse>> GetMeAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.Customer)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new AppException("User not found.", 404);

        var response = new MeResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.ToString(),
            Tier = user.Customer?.Tier.ToString(),
            TotalPoints = user.Customer?.TotalPoints
        };

        return ApiResponse<MeResponse>.SuccessResponse(response);
    }
}
