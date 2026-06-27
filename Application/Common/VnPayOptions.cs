namespace Application.Common;

/// <summary>
/// VNPay gateway settings bound from the "VnPay" configuration section.
/// TmnCode/HashSecret are merchant credentials issued at sandbox.vnpayment.vn and
/// must be supplied via untracked config (e.g. appsettings.Development.json / env),
/// never committed.
/// </summary>
public class VnPayOptions
{
    public string TmnCode { get; set; } = string.Empty;
    public string HashSecret { get; set; } = string.Empty;

    /// <summary>VNPay pay endpoint (sandbox: https://sandbox.vnpayment.vn/paymentv2/vpcpay.html).</summary>
    public string BaseUrl { get; set; } = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";

    /// <summary>Where VNPay redirects the customer's browser after payment.</summary>
    public string ReturnUrl { get; set; } = string.Empty;

    public string Version { get; set; } = "2.1.0";
    public string Command { get; set; } = "pay";
    public string CurrCode { get; set; } = "VND";
    public string Locale { get; set; } = "vn";

    /// <summary>Windows ("SE Asia Standard Time") or IANA ("Asia/Ho_Chi_Minh") id for vnp_CreateDate/ExpireDate.</summary>
    public string TimeZoneId { get; set; } = "SE Asia Standard Time";

    /// <summary>
    /// Minutes a booking may stay unpaid before VNPay expires the request and the
    /// cleanup worker cancels the booking, releasing its reserved slot.
    /// </summary>
    public int PaymentTimeoutMinutes { get; set; } = 15;
}
