namespace Application.DTOs.Payment;

/// <summary>Result shown to the customer after VNPay redirects back (return URL).</summary>
public class PaymentResultResponse
{
    public bool Success { get; set; }
    public string TransactionRef { get; set; } = null!;
    public Guid? BookingId { get; set; }
    public decimal Amount { get; set; }
    public string ResponseCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
