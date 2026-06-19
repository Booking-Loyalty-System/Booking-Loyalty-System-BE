using Application.Common;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;
    public AuthService(IApplicationDbContext context, ITokenService tokenService, IConfiguration config)
    {
        _context = context;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task<ApiResponse<TokenResponse>> RegisterAsync(RegisterRequest request)
    {
        // Check duplicate email
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new AppException("Email already exists.", 409);

        // Check duplicate phone
        if (!string.IsNullOrEmpty(request.PhoneNumber) && await _context.Customers.AnyAsync(c => c.PhoneNumber == request.PhoneNumber))
            throw new AppException("Phone number already exists.", 409);

        var tier = await _context.Tiers.FirstOrDefaultAsync(t => t.TierName == "Bronze")
                   ?? throw new AppException("System configuration error: Default tier not found.", 500);

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
            TierId = tier.Id,
            CreatedAt = DateTime.UtcNow
        };

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        _context.Users.Add(user);
        _context.Customers.Add(customer);
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
            throw new AppException("Account is deactivated.", 401);

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
            TotalPoints = user.Customer?.TotalPoints,
            TotalWashes = user.Customer?.TotalWashes
        };

        return ApiResponse<MeResponse>.SuccessResponse(response);
    }

    public async Task<ApiResponse<TokenResponse>> GoogleLoginAsync(string code)
    {
        try
        {
            var clientId = _config["GoogleKey:ClientId"];
            var clientSecret = _config["GoogleKey:ClientSecret"];

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
            });

            // Đổi Code lấy Token từ Google
            var tokenResponse = await flow.ExchangeCodeForTokenAsync(
                userId: "user", code: code, redirectUri: "postmessage", CancellationToken.None
            );

            var oauthService = new Oauth2Service(new BaseClientService.Initializer
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(tokenResponse.AccessToken),
                ApplicationName = "Your-App-Name"
            });

            // Lấy profile từ Google
            var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();

            if (string.IsNullOrEmpty(userInfo.Email))
                throw new AppException("Cannot retrieve email from Google account.", 400);

            // Tìm xem User đã tồn tại trong hệ thống chưa
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userInfo.Email);
            string message = "Login successful.";

            var tier = await _context.Tiers.FirstOrDefaultAsync(t => t.TierName == "Bronze")
                       ?? throw new AppException("System configuration error: Default tier not found.", 500);
            
            if (user == null)
            {
                // ---- TRƯỜNG HỢP 1: USER CHƯA TỒN TẠI (ĐĂNG KÝ MỚI) ----
                message = "Google registration and login successful.";

                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = userInfo.Email,
                    PasswordHash = null, // Đăng nhập Google nên không có mật khẩu gốc
                    Role = UserRole.Customer,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                    // Thêm trường Avatar/Picture nếu bản User của bạn có hỗ trợ
                };

                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    FullName = userInfo.Name ?? "Google User",
                    PhoneNumber = null, // Chấp nhận NULL lúc này, User cập nhật sau
                    DateOfBirth = null,
                    TierId = tier.Id,
                    CreatedAt = DateTime.UtcNow,
                    User = user
                };

                _context.Users.Add(user);
                _context.Customers.Add(customer);
            }
            else
            {
                // ---- TRƯỜNG HỢP 2: USER ĐÃ TỒN TẠI (ĐĂNG NHẬP) ----
                if (!user.IsActive)
                    throw new AppException("User account is deactivated.", 403);

                // Cập nhật lại thông tin Customer nếu cần
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (customer != null && string.IsNullOrEmpty(customer.FullName))
                {
                    customer.FullName = userInfo.Name;
                    _context.Customers.Update(customer);
                }
            }

            // Tạo cặp AccessToken và RefreshToken nội bộ dựa trên cấu trúc TokenService của bạn
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt = DateTime.UtcNow;

            // Lưu toàn bộ thay đổi xuống DB
            await _context.SaveChangesAsync();

            return ApiResponse<TokenResponse>.SuccessResponse(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60) // Khớp thời gian hết hạn hệ thống bạn
            }, message);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new AppException($"Google authentication failed: {ex.Message}", 500);
        }
    }
}
