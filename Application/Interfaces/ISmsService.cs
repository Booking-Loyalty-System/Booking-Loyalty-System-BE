namespace Application.Interfaces; 

public interface ISmsService
{
    Task<bool> SendOtpAsync(string toPhoneNumber);
    Task<bool> VerifyOtpAsync(string toPhoneNumber, string otpCode);
}