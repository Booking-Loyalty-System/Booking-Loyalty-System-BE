using Application.Common;
using Application.DTOs.Payment;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IApplicationDbContext _context;
    private readonly VnPayOptions _options;
    private readonly TimeZoneInfo _timeZone;

    public PaymentService(IApplicationDbContext context, IOptions<VnPayOptions> options)
    {
        _context = context;
        _options = options.Value;
        _timeZone = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZoneId);
    }

    public async Task<CreatePaymentResponse> CreatePaymentUrlAsync(Guid userId, Guid bookingId, string clientIp)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customer.Id)
            ?? throw new AppException("Booking not found.", 404);

        var alreadyPaid = await _context.Payments
            .AnyAsync(p => p.BookingId == bookingId && p.Status == PaymentStatus.Paid);
        if (alreadyPaid)
            throw new AppException("This booking has already been paid.", 400);

        if (booking.Status != BookingStatus.Pending)
            throw new AppException("Only a pending booking awaiting payment can be paid.", 400);

        var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);

        // Unique per attempt so the IPN resolves exactly one Payment row.
        var txnRef = $"{booking.BookingCode}-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            Method = "VNPAY",
            Gateway = "VNPAY",
            TransactionRef = txnRef,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var vnp = new VnPayLibrary();
        vnp.AddRequestData("vnp_Version", _options.Version);
        vnp.AddRequestData("vnp_Command", _options.Command);
        vnp.AddRequestData("vnp_TmnCode", _options.TmnCode);
        // VNPay expects the amount in the smallest VND unit (x100, no decimals).
        vnp.AddRequestData("vnp_Amount", ((long)(booking.TotalPrice * 100)).ToString());
        vnp.AddRequestData("vnp_CreateDate", nowLocal.ToString("yyyyMMddHHmmss"));
        vnp.AddRequestData("vnp_CurrCode", _options.CurrCode);
        vnp.AddRequestData("vnp_IpAddr", string.IsNullOrWhiteSpace(clientIp) ? "127.0.0.1" : clientIp);
        vnp.AddRequestData("vnp_Locale", _options.Locale);
        vnp.AddRequestData("vnp_OrderInfo", $"Thanh toan booking {booking.BookingCode}");
        vnp.AddRequestData("vnp_OrderType", "other");
        vnp.AddRequestData("vnp_ReturnUrl", _options.ReturnUrl);
        vnp.AddRequestData("vnp_ExpireDate", nowLocal.AddMinutes(_options.PaymentTimeoutMinutes).ToString("yyyyMMddHHmmss"));
        vnp.AddRequestData("vnp_TxnRef", txnRef);

        var payUrl = vnp.CreateRequestUrl(_options.BaseUrl, _options.HashSecret);
        return new CreatePaymentResponse { PayUrl = payUrl, TransactionRef = txnRef };
    }

    public async Task<(string RspCode, string Message)> ProcessIpnAsync(IDictionary<string, string> query)
    {
        var (vnp, inputHash) = LoadResponse(query);
        var (outcome, _) = await FinalizeAsync(vnp, inputHash);

        return outcome switch
        {
            FinalizeOutcome.InvalidSignature => ("97", "Invalid signature"),
            FinalizeOutcome.NotFound => ("01", "Order not found"),
            FinalizeOutcome.InvalidAmount => ("04", "Invalid amount"),
            FinalizeOutcome.AlreadyProcessed => ("02", "Order already confirmed"),
            _ => ("00", "Confirm success") // Success or Failed: both acknowledged to VNPay.
        };
    }

    public async Task<PaymentResultResponse> ProcessReturnAsync(IDictionary<string, string> query)
    {
        // Option A: the browser return also finalises the booking (idempotent, same logic as IPN)
        // so localhost demos work without a public IPN endpoint. In production both arrive; the
        // first wins and the second is a no-op (AlreadyProcessed).
        var (vnp, inputHash) = LoadResponse(query);
        var (outcome, payment) = await FinalizeAsync(vnp, inputHash);

        var success = outcome == FinalizeOutcome.Success
            || (outcome == FinalizeOutcome.AlreadyProcessed && payment?.Status == PaymentStatus.Paid);

        return new PaymentResultResponse
        {
            Success = success,
            TransactionRef = vnp.GetResponseData("vnp_TxnRef"),
            BookingId = payment?.BookingId,
            Amount = payment?.Amount ?? 0m,
            ResponseCode = vnp.GetResponseData("vnp_ResponseCode"),
            Message = outcome switch
            {
                FinalizeOutcome.InvalidSignature => "Invalid signature",
                FinalizeOutcome.NotFound => "Order not found",
                FinalizeOutcome.InvalidAmount => "Invalid amount",
                _ => success ? "Payment successful" : "Payment failed or cancelled"
            }
        };
    }

    private enum FinalizeOutcome { InvalidSignature, NotFound, InvalidAmount, AlreadyProcessed, Success, Failed }

    /// <summary>
    /// Verifies the gateway response and atomically finalises the payment + booking.
    /// Shared by IPN and the browser return so both paths apply identical, idempotent logic.
    /// </summary>
    private async Task<(FinalizeOutcome Outcome, Payment? Payment)> FinalizeAsync(VnPayLibrary vnp, string inputHash)
    {
        if (!vnp.ValidateSignature(inputHash, _options.HashSecret))
            return (FinalizeOutcome.InvalidSignature, null);

        var txnRef = vnp.GetResponseData("vnp_TxnRef");
        var responseCode = vnp.GetResponseData("vnp_ResponseCode");
        var amountRaw = vnp.GetResponseData("vnp_Amount");

        // Serializable so a duplicated callback cannot both pass the "still Pending" check.
        await using var transaction = await _context.BeginTransactionAsync();

        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.TransactionRef == txnRef);
        if (payment is null)
        {
            await transaction.CommitAsync();
            return (FinalizeOutcome.NotFound, null);
        }

        var expected = (long)(payment.Amount * 100);
        if (!long.TryParse(amountRaw, out var paidAmount) || paidAmount != expected)
        {
            await transaction.CommitAsync();
            return (FinalizeOutcome.InvalidAmount, payment);
        }

        // Idempotency: the callback may arrive twice (IPN + return, or VNPay retries).
        if (payment.Status != PaymentStatus.Pending)
        {
            await transaction.CommitAsync();
            return (FinalizeOutcome.AlreadyProcessed, payment);
        }

        payment.TransactionNo = vnp.GetResponseData("vnp_TransactionNo");
        payment.BankCode = vnp.GetResponseData("vnp_BankCode");
        payment.ResponseCode = responseCode;

        if (responseCode == "00")
        {
            payment.Status = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == payment.BookingId);
            if (booking is not null && booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Confirmed;
                booking.UpdatedAt = DateTime.UtcNow;
            }
        }
        else
        {
            payment.Status = PaymentStatus.Failed;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return (responseCode == "00" ? FinalizeOutcome.Success : FinalizeOutcome.Failed, payment);
    }

    /// <summary>Loads every vnp_* param (except the hash fields) into a VnPayLibrary for verification.</summary>
    private static (VnPayLibrary Vnp, string InputHash) LoadResponse(IDictionary<string, string> query)
    {
        var vnp = new VnPayLibrary();
        foreach (var (key, value) in query)
        {
            if (key.StartsWith("vnp_", StringComparison.Ordinal)
                && key != "vnp_SecureHash"
                && key != "vnp_SecureHashType")
            {
                vnp.AddResponseData(key, value);
            }
        }

        var inputHash = query.TryGetValue("vnp_SecureHash", out var h) ? h : string.Empty;
        return (vnp, inputHash);
    }
}
