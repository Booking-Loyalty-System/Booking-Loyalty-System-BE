using Application.Interfaces;
using FirebaseAdmin.Auth;

namespace Infrastructure.Services;

public class FirebaseService : IOtpService
{
    // Interface IOtpService của ông ban đầu có thể cần sửa lại tham số truyền vào một chút 
    // để phù hợp với việc verify IdToken.
    
    public Task<bool> SendOtpAsync(string phoneNumber)
    {
        // BỎ TRỐNG hoặc ném ngoại lệ vì .NET không chịu trách nhiệm gửi mã nữa.
        // Mọi việc gửi mã do Frontend React gọi trực tiếp Firebase SDK.
        throw new NotImplementedException("Firebase Phone Auth gửi OTP trực tiếp từ Client Frontend.");
    }

    /// <summary>
    /// Hàm này dùng để verify IdToken do Google cấp sau khi User nhập đúng OTP ở Frontend
    /// </summary>
    /// <returns>Trả về Số điện thoại nếu Token hợp lệ, trả về null nếu token fake/hết hạn</returns>
    public async Task<string?> VerifyFirebaseTokenAsync(string idToken)
    {
        try
        {
            // Gọi Admin SDK kiểm tra chữ ký token chéo với Google Server
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            
            // Trích xuất số điện thoại nằm trong token bảo mật
            if (decodedToken.Claims.TryGetValue("phone_number", out var phoneNumberObj))
            {
                return phoneNumberObj.ToString(); // Định dạng trả về sẽ có dạng +84...
            }
            
            return null;
        }
        catch
        {
            // Token bị sai, hết hạn hoặc fake
            return null;
        }
    }

    // Giữ nguyên để tránh lỗi Interface nếu ông chưa kịp sửa IOtpService
    public bool VerifyOtp(string phoneNumber, string otpCode)
    {
        throw new NotImplementedException("Dùng hàm VerifyFirebaseTokenAsync thay thế.");
    }
}