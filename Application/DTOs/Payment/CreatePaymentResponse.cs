namespace Application.DTOs.Payment;

public class CreatePaymentResponse
{
    /// <summary>Full VNPay URL (incl. vnp_SecureHash) the client should redirect to.</summary>
    public string PayUrl { get; set; } = null!;

    /// <summary>Our reference for this attempt (vnp_TxnRef).</summary>
    public string TransactionRef { get; set; } = null!;
}
