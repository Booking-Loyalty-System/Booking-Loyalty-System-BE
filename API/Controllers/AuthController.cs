using System.Security.Claims;
using Application.Common;
using Application.DTOs.Auth;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IOtpService _otpService;
    private readonly IMemoryCache _cache;

    public AuthController(IAuthService authService, IOtpService otpService, IMemoryCache cache)
    {
        _authService = authService;
        _otpService = otpService;
        _cache = cache;
    }

    /// <summary>
    /// A01: Register new customer with vehicle
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] IValidator<RegisterRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// A02: Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] IValidator<LoginRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// A03: Refresh access token
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// A04: Change password (requires auth)
    /// </summary>
    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] IValidator<ChangePasswordRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));

        var userId = GetUserId();
        var result = await _authService.ChangePasswordAsync(userId, request);
        return Ok(result);
    }

    /// <summary>
    /// A05: Logout (requires auth)
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = GetUserId();
        var result = await _authService.LogoutAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// A07: Get current user info (requires auth)
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetUserId();
        var result = await _authService.GetMeAsync(userId);
        return Ok(result);
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> LoginGoogle(string code)
    {
        var result = await _authService.GoogleLoginAsync(code);
        return Ok(result);
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp(
        [FromBody] OtpRequest request,
        [FromServices] IValidator<OtpRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));
        
        string phoneNumber = request.PhoneNumber;
        string cooldownKey = $"cooldown:{phoneNumber}";
        string limitKey = $"limit:{phoneNumber}";
        
        // Tầng 1: Tránh spam click liên tục
        if (_cache.TryGetValue(cooldownKey, out _))
        {
            return BadRequest(ApiResponse<object>.FailResponse("Vui lòng đợi 60 giây trước khi yêu cầu mã mới."));
        }

        // Tầng 2: 3 lần trong 1 giờ
        _cache.TryGetValue(limitKey, out int sentCount);
        if (sentCount >= 3)
        {
            return BadRequest(ApiResponse<object>.FailResponse("Số điện thoại này đã vượt quá giới hạn 3 lần gửi OTP trong 1 giờ. Vui lòng thử lại sau."));
        }
        
        // 2. Nếu không bị cooldown, tiến hành gọi eSMS gửi OTP như bình thường
        bool isSent = await _otpService.SendOtpAsync(phoneNumber);
    
        if (isSent)
        {
            var cooldownOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
            _cache.Set(cooldownKey, true, cooldownOptions);
            
            sentCount++;
            var limitOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            _cache.Set(limitKey, sentCount, limitOptions);
            
            return Ok(ApiResponse<object>.SuccessResponse(new { message = "Mã OTP đã được gửi." }));
        }

        return BadRequest(ApiResponse<object>.FailResponse("Gửi SMS thất bại từ hệ thống."));
    }
    
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
        [FromBody] VerifyOtpRequest request,
        [FromServices] IValidator<VerifyOtpRequest> validator)
    {
        // 1. Validate dữ liệu đầu vào của ông (giữ nguyên)
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.FailResponse(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));
    
        // 2. 🔥 SỬA TẠI ĐÂY: Gọi hàm Verify của Firebase thông qua Interface
        // request.OtpCode lúc này chính là cái chuỗi IdToken dài ngoằng từ React gửi lên nha ông
        string? verifiedPhoneNumber = await _otpService.VerifyFirebaseTokenAsync(request.OtpCode);

        if (string.IsNullOrEmpty(verifiedPhoneNumber))
        {
            return BadRequest(ApiResponse<object>.FailResponse("Mã xác thực OTP không hợp lệ hoặc đã hết hạn."));
        }

        // 3. Chuẩn hóa số điện thoại client truyền lên để so sánh chéo bảo mật
        string formattedClientPhone = request.PhoneNumber.StartsWith("0") 
            ? $"+84{request.PhoneNumber.Substring(1)}" 
            : request.PhoneNumber;

        if (verifiedPhoneNumber != formattedClientPhone)
        {
            return BadRequest(ApiResponse<object>.FailResponse("Số điện thoại xác thực không trùng khớp."));
        }

        // 4. Chèn logic EF Core lưu xuống DB (Email = Null) giống như mình bàn hôm trước ở đây...
        // var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == verifiedPhoneNumber);
        // ...

        return Ok(ApiResponse<object>.SuccessResponse(new { 
            message = "Xác thực OTP và đăng ký tài khoản thành công!",
            phoneNumber = verifiedPhoneNumber
        }));
    }
    
    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
