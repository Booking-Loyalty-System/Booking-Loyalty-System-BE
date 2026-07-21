using System;

namespace Application.DTOs.Payment;

public class PaymentAuditRequest
{
    public string? Status { get; set; }
    public string? Search { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
}
