namespace Application.Interfaces;

public interface IOtpService
{
    Task<bool> SendOtpAsync(string phoneNumber);
    bool VerifyOtp(string phoneNumber, string otpCode);
    Task<string?> VerifyFirebaseTokenAsync(string idToken);
}