using Application.DTOs.Auth;
using Application.Common;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<Guid>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<ApiResponse<TokenResponse>> VerifyEmailAsync(Guid Id, string otp);
    Task<ApiResponse<object>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    Task<ApiResponse<object>> LogoutAsync(Guid userId);
    Task<ApiResponse<MeResponse>> GetMeAsync(Guid userId);
    Task<ApiResponse<TokenResponse>> GoogleLoginAsync(string code);
}
