using Application.Common;
using Application.DTOs.Chat;
using Application.DTOs.Feedback;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }
        /*
                [HttpPost("chat")]
                public async Task<IActionResult> ChatWithBot(ChatRequest request)
                {
                    if (string.IsNullOrWhiteSpace(request.Message))
                    {
                        return BadRequest(ApiResponse<object>.FailResponse("Tin nhắn không được để trống."));
                    }

                    try
                    {
                        // Truyền tin nhắn hiện tại kèm lịch sử cuộc trò chuyện (nếu có) sang AIService
                        var botResponse = await _aiService.ChatWithCustomerAsync(request.Message, request.HistoryContext ?? "");

                        var result = new { Reply = botResponse };
                        return Ok(ApiResponse<object>.SuccessResponse(result, "AI phản hồi thành công."));
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ApiResponse<object>.FailResponse($"Lỗi hệ thống AI: {ex.Message}"));
                    }
                }*/

        /// <summary>
        /// API Test chức năng quét và kiểm duyệt nội dung feedback (Moderation)
        /// </summary>
        [HttpPost("moderate-feedback")]
        public async Task<IActionResult> ModerateFeedback([FromBody] ModerationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Comment))
            {
                return BadRequest(ApiResponse<object>.FailResponse("Nội dung bình luận không được để trống."));
            }

            try
            {
                var moderationResult = await _aiService.ModerateFeedbackAsync(request.Comment);

                return Ok(ApiResponse<object>.SuccessResponse(moderationResult, "Kiểm duyệt nội dung hoàn tất."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.FailResponse($"Lỗi hệ thống kiểm duyệt: {ex.Message}"));
            }
        }
    }
}
