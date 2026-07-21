using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// A single payment attempt for a booking through an online gateway (VNPay sandbox).
/// One booking can have several attempts; only one ever reaches <see cref="PaymentStatus.Paid"/>.
/// </summary>
public class Payment
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }

    /// <summary>Amount charged for this attempt (snapshot of the booking's TotalPrice).</summary>
    public decimal Amount { get; set; }

    /// <summary>Payment method (currently always "VNPAY").</summary>
    public string Method { get; set; } = "VNPAY";

    /// <summary>Gateway that processed the payment (currently always "VNPAY").</summary>
    public string Gateway { get; set; } = "VNPAY";

    /// <summary>Our unique reference sent as vnp_TxnRef; VNPay echoes it back on return/IPN.</summary>
    public string TransactionRef { get; set; } = null!;

    /// <summary>VNPay's own transaction id (vnp_TransactionNo), set once the gateway responds.</summary>
    public string? TransactionNo { get; set; }

    /// <summary>Bank code reported by VNPay (vnp_BankCode).</summary>
    public string? BankCode { get; set; }

    /// <summary>Raw VNPay response code (vnp_ResponseCode); "00" means success.</summary>
    public string? ResponseCode { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }

    // Navigation
    public Booking Booking { get; set; } = null!;
}
