using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class SmsService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber;

    public SmsService(IConfiguration config)
    {
        _accountSid = config["TwilioSettings:AccountSid"];
        _authToken = config["TwilioSettings:AuthToken"];
        _fromNumber = config["TwilioSettings:FromPhoneNumber"];
        
        // Khởi tạo client 1 lần
        TwilioClient.Init(_accountSid, _authToken);
    }

    public async Task SendReminderAsync(string toPhoneNumber, string messageBody)
    {
        try
        {
            var message = await MessageResource.CreateAsync(
                body: messageBody,
                from: new Twilio.Types.PhoneNumber(_fromNumber),
                to: new Twilio.Types.PhoneNumber(toPhoneNumber)
            );

            Console.WriteLine($"Gửi tin nhắn thành công! SID: {message.Sid}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi gửi tin: {ex.Message}");
        }
    }
}