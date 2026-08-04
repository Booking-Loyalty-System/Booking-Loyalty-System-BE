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
    private readonly IEmailService _emailService;

    // Cập nhật Constructor để nhận IEmailService
    public AuthService(
        IApplicationDbContext context,
        ITokenService tokenService,
        IConfiguration config,
        IEmailService emailService)
    {
        _context = context;
        _tokenService = tokenService;
        _config = config;
        _emailService = emailService;
    }

    public async Task<ApiResponse<Guid>> RegisterAsync(RegisterRequest request)
    {
        // Check duplicate email
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new AppException("Email already exists.", 409);

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
        var point = new Point()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TotalPoints = 0,
            AvailablePoints = 0
        };

        var otpCode = new Random().Next(100000, 999999).ToString(); // Sinh mã OTP 6 chữ số
        var emailVerification = new EmailVerification
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Email = request.Email,
            VerificationToken = otpCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15), // Hết hạn sau 15 phút
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        // Gán nav để token kèm được fullName (claim trong JWT).
        user.Customer = customer;

        _context.Users.Add(user);
        _context.Customers.Add(customer);
        _context.Points.Add(point);
        _context.EmailVerifications.Add(emailVerification);
        await _context.SaveChangesAsync();

        var emailSubject = "Xác Thực Địa Chỉ Email Của Bạn";
        var emailBody = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                <div style='background-color: #007bff; color: #ffffff; padding: 20px; text-align: center;'>
                    <h2 style='margin: 0;'>Chào mừng đến với hệ thống!</h2>
                </div>
                <div style='padding: 24px; color: #333333;'>
                    <p>Xin chào <strong>{request.FullName}</strong>,</p>
                    <p>Cảm ơn bạn đã đăng ký tài khoản. Để hoàn tất quy trình xác thực email, vui lòng sử dụng mã OTP dưới đây:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='font-size: 32px; font-weight: bold; letter-spacing: 6px; color: #007bff; background: #f0f7ff; padding: 10px 24px; border-radius: 4px; border: 1px dashed #007bff;'>
                            {otpCode}
                        </span>
                    </div>
                    <p style='color: #ee5253;'><strong>Lưu ý:</strong> Mã xác thực này có hiệu lực trong vòng <strong>15 phút</strong>. Vui lòng không chia sẻ mã này cho bất kỳ ai khác.</p>
                </div>
                <div style='background-color: #f8f9fa; text-align: center; padding: 12px; color: #777777; font-size: 12px; border-top: 1px solid #e0e0e0;'>
                    Đây là email tự động. Vui lòng không phản hồi lại email này.
                </div>
            </div>";

        try
        {
            await _emailService.SendEmailAsync(request.Email, emailSubject, emailBody);
        }
        catch (Exception ex)
        {
            throw new AppException($"Đăng ký hoàn tất nhưng không thể gửi email kích hoạt: {ex.Message}", 500);
        }

        return ApiResponse<Guid>.SuccessResponse(user.Id, "Registration successful. Please check your email for the verification code.");
    }

    public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Customer)
            .Include(u => u.Staff)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AppException("Invalid email or password.", 401);

        if (!user.IsActive)
            throw new AppException("Account is deactivated.", 401);

        if (!user.IsEmailConfirmed)
            throw new AppException("Please verify your email address before logging in.", 403);

        if (user.Staff != null && !user.Staff.IsAvailable)
            throw new AppException("Your staff account has been disabled.", 403);

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

    public async Task<ApiResponse<TokenResponse>> VerifyEmailAsync(Guid id, string otp)
    {
        var user = await _context.Users
         .Include(u => u.Customer)
         .Include(u => u.Staff)
         .FirstOrDefaultAsync(u => u.Id == id)
         ?? throw new AppException("User not found.", 404);

        if (user.IsEmailConfirmed)
            throw new AppException("This email has already been verified.", 400);

        // Tìm bản ghi OTP mới nhất, chưa sử dụng của email này
        var verification = await _context.EmailVerifications
         .Where(ev => ev.UserId == id && !ev.IsUsed)
         .OrderByDescending(ev => ev.CreatedAt)
         .FirstOrDefaultAsync()
         ?? throw new AppException("No verification code request found.", 404);

        // Kiểm tra OTP có khớp hay không
        if (verification.VerificationToken != otp)
            throw new AppException("Invalid verification code.", 400);

        // Kiểm tra mã OTP đã hết hạn chưa
        if (verification.ExpiresAt < DateTime.UtcNow)
            throw new AppException("Verification code has expired.", 400);

        // Đánh dấu đã sử dụng OTP và xác thực email thành công
        verification.IsUsed = true;
        verification.VerifiedAt = DateTime.UtcNow;
        user.IsEmailConfirmed = true;
        user.UpdatedAt = DateTime.UtcNow;

        // Sinh luôn token đăng nhập trực tiếp cho người dùng sau khi xác thực thành công
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return ApiResponse<TokenResponse>.SuccessResponse(new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60)
        }, "Email verified successfully. Welcome to system!");
    }

    public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Customer)
            .Include(u => u.Staff)
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

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
                .ThenInclude(c => c.Tier) // 🔥 FIX LỖI: Cần nạp bảng Tier để không bị NullReferenceException
            .Include(u => u.Staff)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new AppException("User not found.", 404);

        var point = await _context.Points.FirstOrDefaultAsync(p => p.UserId == userId);

        var response = new MeResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.ToString(),
            FullName = user.Customer?.FullName ?? user.Staff?.FullName,
            Tier = user.Customer?.Tier?.TierName, // Sửa lại lấy đúng thuộc tính tên Hạng (ví dụ TierName)
            TotalPoints = point?.TotalPoints,
            TotalWashes = user.Customer?.TotalWashes,
            AvailablePoints = point?.AvailablePoints
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
                    IsEmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    FullName = userInfo.Name ?? "Google User",
                    PhoneNumber = null,
                    DateOfBirth = null,
                    TierId = tier.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var point = new Point
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    TotalPoints = 0,
                    AvailablePoints = 0
                };

                _context.Users.Add(user);
                _context.Customers.Add(customer);
                _context.Points.Add(point);
            }
            else
            {
                // ---- TRƯỜNG HỢP 2: USER ĐÃ TỒN TẠI (ĐĂNG NHẬP) ----
                if (!user.IsActive)
                    throw new AppException("User account is deactivated.", 403);

                if (!user.IsEmailConfirmed)
                {
                    user.IsEmailConfirmed = true;
                }

                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (customer != null && string.IsNullOrEmpty(customer.FullName))
                {
                    customer.FullName = userInfo.Name;
                    _context.Customers.Update(customer);
                }
            }

            // Tạo cặp AccessToken và RefreshToken nội bộ
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt = DateTime.UtcNow;

            // Lưu toàn bộ thay đổi xuống DB Postgres
            await _context.SaveChangesAsync();

            return ApiResponse<TokenResponse>.SuccessResponse(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60)
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
