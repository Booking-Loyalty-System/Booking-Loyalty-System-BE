using Application.DTOs.Payment;

namespace Application.Interfaces;

public interface IPaymentService
{
    /// <summary>Creates a Pending payment attempt and returns a signed VNPay redirect URL.</summary>
    Task<CreatePaymentResponse> CreatePaymentUrlAsync(Guid userId, Guid bookingId, string clientIp);

    /// <summary>
    /// Handles VNPay's server-to-server IPN (the source of truth): verifies the
    /// signature and amount, finalises the payment, and confirms the booking.
    /// Returns the (RspCode, Message) pair VNPay expects as JSON.
    /// </summary>
    Task<(string RspCode, string Message)> ProcessIpnAsync(IDictionary<string, string> query);

    /// <summary>Verifies the browser return URL and maps it to a display result (no DB writes).</summary>
    Task<PaymentResultResponse> ProcessReturnAsync(IDictionary<string, string> query);
}
