using Application.Common;
using Application.DTOs.Feedback;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpPost("customer/submit")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateFeedback(FeedbackRequest dto)
        {
            var userId = GetUserId();

            try
            {
                var result = await _service.CustomerCreateFeedbackAsync(userId, dto);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("public/all")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _service.GetAllFeedbacksAsync();
            return Ok(new { success = true, data = result });
        }

        [Authorize] // Hoặc [Authorize(Roles = "Admin,Manager")] tùy vào luồng phân quyền
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredFeedbacks(
    [FromQuery] string? sortBy = "newest",
    [FromQuery] bool? isGifted = null)
        {
            // Truyền đầy đủ các tham số filter và sort xuống service xử lý
            var result = await _service.GetFeedbacksAsync(sortBy, isGifted);

            return Ok(ApiResponse<object>.SuccessResponse(result, "Lấy danh sách đánh giá thành công."));
        }

        [Authorize]
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] int topCount = 5)
        {
            var result = await _service.GetFeedbackStatisticsAsync(topCount);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Lấy dữ liệu thống kê thành công."));
        }
        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(claim!);
        }
    }
}
