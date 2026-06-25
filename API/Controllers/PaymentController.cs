using System.Security.Claims;
using Application.Common;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayOS;
using PayOS.Models.Webhooks;

namespace API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly PayOSClient _payOSClient;
    public PaymentController(IPaymentService paymentService, IOptions<PayOsOptions> payOSOptions)
    {
        _paymentService = paymentService;
        _payOSClient = new PayOSClient(
            payOSOptions.Value.ClientId,
            payOSOptions.Value.ApiKey,
            payOSOptions.Value.ChecksumKey
        );
    }

    [Authorize]
    [HttpPost("bookings/{bookingId:guid}/checkout")]
    public async Task<IActionResult> Checkout(Guid bookingId)
    {
        var userId = GetUserId();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        var result = await _paymentService.CreatePaymentUrlAsync(userId, bookingId, ip);
        return Ok(ApiResponse<object>.SuccessResponse(result, "Payment URL created."));
    }

    // VNPay server-to-server IPN: the source of truth. Returns the JSON VNPay expects.
    [AllowAnonymous]
    [HttpGet("vnpay/ipn")]
    public async Task<IActionResult> Ipn()
    {
        var query = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        var (rspCode, message) = await _paymentService.ProcessIpnAsync(query);
        return Ok(new { RspCode = rspCode, Message = message });
    }

    // VNPay browser return URL: display-only, used by the frontend to show the result.
    [AllowAnonymous]
    [HttpGet("vnpay/return")]
    public async Task<IActionResult> Return()
    {
        var query = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        var result = await _paymentService.ProcessReturnAsync(query);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
        /// Endpoint 1: Tạo link thanh toán PayOS
        /// URL: POST /api/payments/payos/create-link
        /// </summary>
        [HttpPost("payos/{bookingId}/create-link")]
        public async Task<IActionResult> CreatePayOsLink(Guid bookingId)
        {
            try
            {
                var userId = GetUserId();
                var response = await _paymentService.CreatePayOSPaymentAsync(userId, bookingId);

                return Ok(response);
            }
            catch (AppException ex) 
            {
                return StatusCode(ex.StatusCode, new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint 2: Nhận Webhook từ PayOS khi khách quét mã xong
        /// URL: POST /api/payments/payos/webhook
        /// </summary>
        [HttpPost("payos/webhook")]
        public async Task<IActionResult> PayOsWebhook([FromBody] Webhook webhookBody)
        {
            try
            {
                // ✅ Đã sửa: Dùng Webhooks.VerifyAsync để xác thực chữ ký và bóc tách dữ liệu sạch
                WebhookData webhookData = await _payOSClient.Webhooks.VerifyAsync(webhookBody);

                // Cập nhật trạng thái Booking thành Confirmed và Payment thành Paid trong DB
                await _paymentService.HandlePayOsWebhookAsync(webhookData);

                // Bắt buộc trả về HTTP 200 để báo cho PayOS dừng gửi lại gói tin này
                return Ok(new { success = true, message = "Webhook handled successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PayOS Webhook Error]: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        [HttpGet("return")]
        public async Task<IActionResult> PaymentReturn(
            [FromQuery] string code, 
            [FromQuery] string status, 
            [FromQuery] string orderCode)
        {
            // Kiểm tra xem orderCode truyền về có hợp lệ không
            if (!long.TryParse(orderCode, out long parsedOrderCode))
            {
                return Redirect("http://localhost:5173/staff/dashboard?paymentStatus=failed");
            }

            // Gọi sang service cập nhật Database ngay tại đây
            bool isSuccess = await _paymentService.ProcessPayOsReturnAsync(parsedOrderCode, status, code);

            // Chuyển hướng người dùng về Frontend kèm theo trạng thái để hiển thị Toast
            if (isSuccess)
            {
                return Redirect($"http://localhost:5173/staff/dashboard?paymentStatus=success&orderCode={orderCode}");
            }
        
            return Redirect($"http://localhost:5173/staff/dashboard?paymentStatus=cancel&orderCode={orderCode}");
        }

        // Endpoint nếu họ bấm nút Hủy trên trang PayOS: https://localhost:7001/api/payment/cancel
        [HttpGet("cancel")]
        public IActionResult PaymentCancel([FromQuery] string orderCode)
        {
            return Redirect($"http://localhost:5173/staff/dashboard?paymentStatus=cancel&orderCode={orderCode}");
        }
    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(claim!);
    }
}
