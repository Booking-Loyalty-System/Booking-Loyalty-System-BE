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

        /* [HttpPut("staff/reply/{feedbackId}")]
         [Authorize(Roles = "Staff,Admin")]
         public async Task<IActionResult> ReplyFeedback(Guid feedbackId, ReplyFeedbackRequest dto)
         {
             var userId = GetUserId();
             try
             {
                 var result = await _service.StaffReplyFeedbackAsync(userId, feedbackId, dto);
                 return Ok(new { success = true, message = "Phản hồi đánh giá thành công.", data = result });
             }
             catch (Exception ex)
             {
                 return BadRequest(new { success = false, message = ex.Message });
             }
         }*/

        [HttpGet("public/all")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _service.GetAllFeedbacksAsync();
            return Ok(new { success = true, data = result });
        }

        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(claim!);
        }
    }
}
