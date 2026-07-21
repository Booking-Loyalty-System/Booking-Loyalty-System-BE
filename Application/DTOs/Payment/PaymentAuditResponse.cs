using System;

namespace Application.DTOs.Payment;

public class PaymentAuditItem
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Gateway { get; set; } = string.Empty;
    public string TransactionRef { get; set; } = string.Empty;
    public string? TransactionNo { get; set; }
    public string? BankCode { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
}

public class PaymentAuditResponse
{
    public List<PaymentAuditItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
}
