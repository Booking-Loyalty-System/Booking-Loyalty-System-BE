using Application.DTOs.ChatFeedback;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatFeedbackController : ControllerBase
    {
        private readonly IChatFeedbackService _chatFeedbackService;

        public ChatFeedbackController(IChatFeedbackService chatFeedbackService)
        {
            _chatFeedbackService = chatFeedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateChatFeedbackRequest request)
        {
            // Kiểm tra tính hợp lệ của Model State (Validation Attributes nếu có)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _chatFeedbackService.CreateChatFeedbackAsync(request);

            if (!result)
            {
                return BadRequest(new { message = "Không thể lưu đánh giá. Vui lòng thử lại sau." });
            }

            return Ok(new { message = "Gửi đánh giá cuộc trò chuyện thành công!" });
        }

        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<ChatFeedbackResponse>>> GetLatestChatFeedbacks([FromQuery] int count = 10)
        {
            var result = await _chatFeedbackService.GetLatestChatFeedbacksAsync(count);
            return Ok(result);
        }

        [HttpGet("staff-statistics")]
        public async Task<ActionResult<ChatStaffStatisticResponse>> GetTopChatStaff([FromQuery] int topCount = 5)
        {
            var result = await _chatFeedbackService.GetTopChatStaffAsync(topCount);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ChatFeedbackDetailResponse>> GetChatFeedbackDetail(Guid id)
        {
            var result = await _chatFeedbackService.GetChatFeedbackDetailAsync(id);
            return Ok(result);
        }

    }
}
