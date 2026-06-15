using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.ExternalAPI;

public class SpeedSMSAPI
{
    private readonly string _accessToken;
    private static readonly HttpClient _httpClient = new HttpClient();

    public SpeedSMSAPI(string token)
    {
        _accessToken = token;
    }

    public async Task<string> CreatePinAsync(string phone, string appId, string content)
    {
        var url = "https://api.speedsms.vn/index.php/pin/create";
    
        // Bổ sung thêm trường content vào JSON payload
        var payload = new 
        { 
            to = phone, 
            app_id = appId,
            content = content 
        };
    
        return await SendRequestAsync(url, payload);
    }

    public async Task<string> VerifyPinAsync(string phone, string appId, string pinCode)
    {
        var url = "https://api.speedsms.vn/index.php/pin/verify";
        var payload = new { to = phone, app_id = appId, pin_code = pinCode };
        return await SendRequestAsync(url, payload);
    }

    // Hàm dùng chung để gửi HTTP Request cho gọn code
    private async Task<string> SendRequestAsync(string url, object payload)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        var authString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_accessToken}:x"));
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);
        request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}