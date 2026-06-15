using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Application.Interfaces; 
using Infrastructure.ExternalAPI; 

namespace Infrastructure.Services;

public class SmsService : ISmsService 
{
    private readonly string _accessToken;
    private readonly string _appId;

    public SmsService(IConfiguration config)
    {
        _accessToken = config["SpeedSmsSettings:AccessToken"];
        _appId = config["SpeedSmsSettings:AppId"];
    }

    public async Task<bool> SendOtpAsync(string toPhoneNumber)
    {
        try
        {
            var api = new SpeedSMSAPI(_accessToken);
        
            // BẮT BUỘC phải có từ khóa {pin_code}, SpeedSMS sẽ tự thay thế nó thành mã OTP (ví dụ: 1234)
            string content = "Ma xac thuc OTP cua ban la {pin_code}. Chuc ban mot ngay tot lanh!"; 
        
            // Truyền thêm biến content vào đây
            string response = await api.CreatePinAsync(toPhoneNumber, _appId, content);
        
            var jsonResult = JObject.Parse(response);
            if (jsonResult["status"]?.ToString() == "success")
            {
                Console.WriteLine("Gửi OTP qua 2FA App thành công!");
                return true;
            }
        
            Console.WriteLine($"Lỗi gửi OTP: {jsonResult["message"]}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> VerifyOtpAsync(string toPhoneNumber, string otpCode)
    {
        try
        {
            var api = new SpeedSMSAPI(_accessToken);
            string response = await api.VerifyPinAsync(toPhoneNumber, _appId, otpCode);
            
            var jsonResult = JObject.Parse(response);
            // SpeedSMS trả về status success nếu mã đúng
            if (jsonResult["status"]?.ToString() == "success")
            {
                return true;
            }
            Console.WriteLine($"Lỗi xác thực: {jsonResult["message"]}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
            return false;
        }
    }
}