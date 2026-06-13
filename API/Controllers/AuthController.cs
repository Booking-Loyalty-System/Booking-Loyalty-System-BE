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
    
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
        [FromBody] VerifyOtpRequest request,
        [FromServices] IValidator<VerifyOtpRequest> validator,
            [FromServices] ILogger<AuthController> logger)
    {
        logger.LogInformation("==> [OTP VERIFY] Nhận yêu cầu xác thực cho SĐT: {PhoneNumber}", request.PhoneNumber);
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errorMessages = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            
            logger.LogWarning("--> [OTP VALIDATION FAILED] SĐT: {PhoneNumber}. Lý do: {Errors}", request.PhoneNumber, errorMessages);
            return BadRequest(ApiResponse<object>.FailResponse(errorMessages));
        }

        logger.LogInformation("--> [OTP FIREBASE] Đang gửi Token lên Firebase để xác thực...");
        string? verifiedPhoneNumber = await _otpService.VerifyFirebaseTokenAsync(request.OtpCode);

        if (string.IsNullOrEmpty(verifiedPhoneNumber))
        {
            logger.LogError("--> [OTP FIREBASE FAILED] Token không hợp lệ hoặc đã hết hạn. SĐT gửi lên: {PhoneNumber}", request.PhoneNumber);
            
            return BadRequest(ApiResponse<object>.FailResponse("Mã xác thực Firebase (Token) không hợp lệ hoặc đã hết hạn."));
        }
        
        string formattedClientPhone = request.PhoneNumber.StartsWith("0") 
            ? $"+84{request.PhoneNumber.Substring(1)}" 
            : request.PhoneNumber;

        if (verifiedPhoneNumber != formattedClientPhone)
        {
            logger.LogWarning("--> [OTP MISMATCH] SĐT từ Token ({VerifiedPhone}) KHÔNG KHỚP với SĐT từ Request ({ClientPhone})", 
                verifiedPhoneNumber, formattedClientPhone);
            return BadRequest(ApiResponse<object>.FailResponse("Số điện thoại trong token không trùng khớp với dữ liệu gửi lên."));
        }
        logger.LogInformation("==> [OTP SUCCESS] Xác thực Firebase thành công cho SĐT: {PhoneNumber}", verifiedPhoneNumber);
        return Ok(ApiResponse<object>.SuccessResponse(new { 
            message = "Xác thực Firebase và đăng ký tài khoản thành công!",
            phoneNumber = verifiedPhoneNumber
        }));
    }
    
    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
