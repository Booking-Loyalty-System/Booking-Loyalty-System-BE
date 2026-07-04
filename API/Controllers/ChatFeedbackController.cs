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
    }
}
