using Application.Common;
using Application.DTOs.Chat;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/chat")]
[Authorize(Roles = "Staff,Admin")]
public class AdminChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public AdminChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        // userId is retrieved from claims
        var userIdClaim = User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse<object>.FailResponse("Invalid user."));
        // Admin sends message to a given session. The incoming DTO contains Message;
        // SessionId should be included in route or extended DTO; assume message targets an active session
        // For backward compatibility, require SessionId be passed as a query parameter 'sessionId'
        var sessionIdQuery = Request.Query["sessionId"].FirstOrDefault();
        if (!Guid.TryParse(sessionIdQuery, out var sessionId))
            return BadRequest(ApiResponse<object>.FailResponse("Missing or invalid sessionId query parameter."));

        var message = await _chatService.StaffSendMessageAsync(userId, sessionId, request.Message);
        return Ok(ApiResponse<object>.SuccessResponse(message));
    }

    [HttpPost("accept/{sessionId:guid}")]
    public async Task<IActionResult> AcceptSession(Guid sessionId)
    {
        var userIdClaim = User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse<object>.FailResponse("Invalid user."));

        var session = await _chatService.AcceptChatSessionAsync(userId, sessionId);
        return Ok(ApiResponse<object>.SuccessResponse(session));
    }

    [HttpPost("close/{sessionId:guid}")]
    public async Task<IActionResult> CloseSession(Guid sessionId)
    {
        var success = await _chatService.CloseChatSessionAsync(sessionId);
        return Ok(ApiResponse<object>.SuccessResponse(success));
    }
}
