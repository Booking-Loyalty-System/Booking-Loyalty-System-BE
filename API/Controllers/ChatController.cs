using Application.Common;
using Application.DTOs.Chat;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly IChatService _chatService;

        public ChatController(IAIService aiService, IChatService chatService)
        {
            _aiService = aiService;
            _chatService = chatService;
        }

        [HttpPost("customer/send")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerSendMessage(SendMessageRequest request)
        {
            var userId = GetUserId();

            try
            {
                var (sessionId, responseMessage) = await _aiService.ChatWithCustomerAsync(userId, request.Message);
                return Ok(new
                {
                    chatSessionId = sessionId,
                    reply = responseMessage
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("staff/waiting-list")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> GetWaitingSessions()
        {
            var sessions = await _chatService.GetWaitingSessionsAsync();
            return Ok(sessions);
        }

        [HttpPost("staff/accept/{sessionId}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> AcceptSession(Guid sessionId)
        {
            var userId = GetUserId();

            try
            {
                var result = await _chatService.AcceptChatSessionAsync(userId, sessionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("staff/send/{sessionId}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> StaffSendMessage(Guid sessionId, SendMessageRequest request)
        {
            var userId = GetUserId();

            try
            {
                var result = await _chatService.StaffSendMessageAsync(userId, sessionId, request.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("staff/close/{sessionId}")]
        [Authorize]
        public async Task<IActionResult> CloseSession(Guid sessionId)
        {
            var success = await _chatService.CloseChatSessionAsync(sessionId);
            if (!success) return BadRequest("Không thể đóng phiên trò chuyện.");
            return Ok(new { message = "Đã đóng phiên hỗ trợ thành công." });
        }

        [HttpPut("customer/toggle-status")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ToggleChatStatus(string target)
        {
            var userId = GetUserId();// Lấy từ Claims

            try
            {
                var updatedSession = await _chatService.ToggleSessionStatusAsync(userId, target);

                return Ok(ApiResponse<object>.SuccessResponse(new
                {
                    chatSessionId = updatedSession.Id,
                    currentStatus = updatedSession.Status
                }, "Chuyển đổi trạng thái thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }

        [HttpGet("staff/active-session")]
        [Authorize]
        public async Task<IActionResult> GetStaffAllSession()
        {
            var userId = GetUserId();

            try
            {
                var activeSessions = await _chatService.GetActiveSessionsByStaffAsync(userId);

                return Ok(ApiResponse<object>.SuccessResponse(activeSessions, "Lấy danh sách phiên trò chuyện đang hoạt động thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }

        [HttpGet("customer/history")]
        [Authorize] // Đảm bảo chỉ Customer đã đăng nhập mới gọi được lịch sử của chính họ
        public async Task<IActionResult> GetCustomerChatHistory()
        {
            var userId = GetUserId();

            try
            {
                var historySessions = await _chatService.GetCustomerChatHistoryAsync(userId);

                return Ok(ApiResponse<object>.SuccessResponse(historySessions, "Lấy lịch sử phiên trò chuyện thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailResponse(ex.Message));
            }
        }
        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(claim!);
        }
    }
}
