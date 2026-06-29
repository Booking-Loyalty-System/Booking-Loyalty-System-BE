using Application.Common;
using Application.DTOs.Payment;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IApplicationDbContext _context;
    private readonly VnPayOptions _options;
    private readonly TimeZoneInfo _timeZone;
    private readonly PayOsOptions _payOSOptions;
    private readonly ILoyaltyService _service;
    private readonly IHubContext<BookingHub> _hubContext;

    public PaymentService(IApplicationDbContext context, VnPayOptions options, TimeZoneInfo timeZone, PayOsOptions payOSOptions, ILoyaltyService service, IHubContext<BookingHub> hubContext)
    {
        _context = context;
        _options = options;
        _timeZone = timeZone;
        _payOSOptions = payOSOptions;
        _service = service;
        _hubContext = hubContext;
    }

    public async Task<CreatePaymentResponse> CreatePaymentUrlAsync(Guid userId, Guid bookingId, string clientIp)
    {
        // 1. TÌM BOOKING TRƯỚC BẰNG BOOKING ID
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        // 2. TÌM CUSTOMER TỪ BOOKING
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == booking.CustomerId)
            ?? throw new AppException("Customer profile not found.", 404);

        var alreadyPaid = await _context.Payments
            .AnyAsync(p => p.BookingId == bookingId && p.Status == PaymentStatus.Paid);
        if (alreadyPaid)
            throw new AppException("This booking has already been paid.", 400);

        // ĐIỀU KIỆN 1: CHỈ CHO PHÉP TRẠNG THÁI COMPLETED MỚI ĐƯỢC TẠO LINK THANH TOÁN
        if (booking.Status != BookingStatus.Completed)
            throw new AppException("Only a completed booking awaiting payment can be paid.", 400);

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

        // SỬA LỖI LÀM TRÒN: Dùng Math.Round để tránh sai số dấu phẩy động làm lệch chữ ký số tiền
        var vnpAmount = Convert.ToInt64(Math.Round(booking.TotalPrice * 100)).ToString();
        vnp.AddRequestData("vnp_Amount", vnpAmount);

        vnp.AddRequestData("vnp_CreateDate", nowLocal.ToString("yyyyMMddHHmmss"));
        vnp.AddRequestData("vnp_CurrCode", _options.CurrCode);
        vnp.AddRequestData("vnp_IpAddr", string.IsNullOrWhiteSpace(clientIp) ? "127.0.0.1" : clientIp);
        vnp.AddRequestData("vnp_Locale", _options.Locale);

        // SỬA LỖI CHỮ KÝ: Thay khoảng trắng bằng dấu gạch dưới để tránh lỗi lệch UrlEncode giữa code và VNPay
        vnp.AddRequestData("vnp_OrderInfo", $"Thanh_toan_booking_{booking.BookingCode}");

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
            _ => ("00", "Confirm success")
        };
    }

    public async Task<PaymentResultResponse> ProcessReturnAsync(IDictionary<string, string> query)
    {
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

    private async Task<(FinalizeOutcome Outcome, Payment? Payment)> FinalizeAsync(VnPayLibrary vnp, string inputHash)
    {
        if (!vnp.ValidateSignature(inputHash, _options.HashSecret))
            return (FinalizeOutcome.InvalidSignature, null);

        var txnRef = vnp.GetResponseData("vnp_TxnRef");
        var responseCode = vnp.GetResponseData("vnp_ResponseCode");
        var amountRaw = vnp.GetResponseData("vnp_Amount");

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

            // ĐIỀU KIỆN 2: ĐỒNG NHẤT LOGIC - Kiểm tra trạng thái Completed lúc này để cập nhật thành Confirmed
            if (booking is not null && booking.Status == BookingStatus.Completed)
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

    public async Task<PaymentResponse> CreatePayOSPaymentAsync(Guid userId, Guid bookingId)
    {
        // 1. Tìm Booking
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId)
            ?? throw new AppException("Booking not found.", 404);

        // 2. Kiểm tra nghiệp vụ (VD: Chỉ cho phép trạng thái Completed mới được thanh toán)
        if (booking.Status != BookingStatus.Completed)
            throw new AppException("Only a completed booking awaiting payment can be paid.", 400);

        // 3. Kiểm tra xem đã thanh toán thành công chưa
        var alreadyPaid = await _context.Payments
            .AnyAsync(p => p.BookingId == bookingId && p.Status == PaymentStatus.Paid);
        if (alreadyPaid)
            throw new AppException("This booking has already been paid.", 400);

        // 4. Tạo OrderCode (PayOS yêu cầu orderCode dạng số kiểu int/long - max 53 bit)
        // Dùng UnixTimeMilliseconds là giải pháp an toàn và unique nhất
        long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // 5. Build payload gửi lên PayOS
        var description = $"Booking {booking.BookingCode}";
        if (description.Length > 25)
        {
            description = description[..25];
        }

        var payOS = new PayOSClient(
            _payOSOptions.ClientId,
            _payOSOptions.ApiKey,
            _payOSOptions.ChecksumKey
        );


        var requestData = new CreatePaymentLinkRequest()
        {
            OrderCode = orderCode,
            Amount = (int)Math.Round(booking.TotalPrice),
            Description = description,
            ReturnUrl = _payOSOptions.ReturnUrl,
            CancelUrl = _payOSOptions.CancelUrl
        };

        // 6. Gọi SDK tạo link thanh toán
        var response = await payOS.PaymentRequests.CreateAsync(requestData);

        // 7. Map chính xác vào Domain.Entities.Payment của bạn
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            Method = "PAYOS",
            Gateway = "PAYOS",
            TransactionRef = orderCode.ToString(),
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        // 8. Trả về CheckoutUrl 
        return new PaymentResponse
        {
            CheckoutUrl = response.CheckoutUrl // Hoặc response.CheckoutUrl tùy phiên bản SDK của bạn ghi hoa/thường
        };
    }

    public async Task HandlePayOsWebhookAsync(WebhookData data)
    {
        await using var transaction = await _context.BeginTransactionAsync();

        try
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionRef == data.OrderCode.ToString())
                ?? throw new Exception("Payment not found");

            // 🔐 Anti fake: Kiểm tra xem số tiền PayOS báo về có khớp với DB không
            if (payment.Amount != data.Amount)
                throw new Exception("Amount mismatch");

            // 🔁 Idempotent: Chống xử lý trùng. Nếu khác Pending (đã Paid hoặc Failed rồi) thì ngừng.
            if (payment.Status != PaymentStatus.Pending)
            {
                await transaction.RollbackAsync();
                return;
            }

            // ❌ Thanh toán thất bại (Mã "00" của PayOS là thành công)
            if (data.Code != "00")
            {
                payment.Status = PaymentStatus.Failed;
                _context.Payments.Update(payment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return;
            }

            // ✅ Thanh toán thành công
            payment.Status = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == payment.BookingId);

            if (booking is not null && booking.Status == BookingStatus.Completed)
            {
                booking.Status = BookingStatus.CheckedOut;
                booking.UpdatedAt = DateTime.UtcNow;
            }

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ProcessPayOsReturnAsync(long orderCode, string status, string code)
    {
        await using var transaction = await _context.BeginTransactionAsync();
        Guid? bookingIdToAward = null;
        Guid? washBayIdToRelease = null;
        try
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionRef == orderCode.ToString());

            if (payment is null) return false;

            // Nếu đã xử lý trước đó rồi (ví dụ reload trang) thì trả về kết quả hiện tại luôn
            if (payment.Status != PaymentStatus.Pending)
            {
                return payment.Status == PaymentStatus.Paid;
            }

            // ❌ Nếu trạng thái từ PayOS báo về thất bại hoặc hủy
            if (code != "00" || status == "CANCELLED")
            {
                payment.Status = PaymentStatus.Failed;
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _hubContext.Clients.All.SendAsync("ReceivePayment", payment.Amount, "Cancelled");

                return false;
            }

            // ✅ Thanh toán thành công (code == "00" và status == "PAID")
            payment.Status = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == payment.BookingId);

            if (booking is not null && booking.Status == BookingStatus.Completed)
            {
                booking.Status = BookingStatus.CheckedOut;
                booking.UpdatedAt = DateTime.UtcNow;

                bookingIdToAward = booking.Id;
                washBayIdToRelease = booking.BayId;
                await _hubContext.Clients.All.SendAsync("ReceivePayment", payment.Amount, "Paid");
            }

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        if (bookingIdToAward.HasValue)
        {
            await _service.AwardPointsForBookingAsync(bookingIdToAward.Value);
        }
        if (washBayIdToRelease.HasValue && bookingIdToAward.HasValue)
        {
            await CheckAndReleaseWashBayAsync(washBayIdToRelease.Value, bookingIdToAward.Value);
            await _context.SaveChangesAsync();
        }
        return true;
    }

    private async Task CheckAndReleaseWashBayAsync(Guid? washBayId, Guid currentBookingId)
    {
        if (!washBayId.HasValue) return;

        // Một khoang vẫn Busy nếu còn xe CheckedIn, Queued hoặc InProgress (loại trừ xe hiện tại)
        bool hasActiveBookings = await _context.Bookings
            .AnyAsync(b => b.BayId == washBayId
                           && b.Id != currentBookingId
                           && (b.Status == BookingStatus.CheckedIn ||
                               b.Status == BookingStatus.Queued ||
                               b.Status == BookingStatus.InProgress));

        if (!hasActiveBookings)
        {
            var washBay = await _context.WashBays.FindAsync(washBayId);
            if (washBay != null)
            {
                washBay.Status = WashBayStatus.Available;
                _context.WashBays.Update(washBay);
            }
        }
    }
}
