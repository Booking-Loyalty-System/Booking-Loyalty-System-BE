using Application.DTOs.Chat;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatSessionResponse>> GetWaitingSessionsAsync();
        Task<ChatSessionResponse> AcceptChatSessionAsync(Guid userId, Guid sessionId);
        Task<ChatMessageResponse> StaffSendMessageAsync(Guid userId, Guid sessionId, string message);
        Task<bool> CloseChatSessionAsync(Guid sessionId);
        Task<ChatSession> ToggleSessionStatusAsync(Guid userId, string target);
        Task<List<ChatSessionResponse>> GetActiveSessionsByStaffAsync(Guid userId);
        Task<IEnumerable<ChatSessionResponse>> GetCustomerChatHistoryAsync(Guid userId);
        Task<IEnumerable<ChatSessionResponse>> GetAllChatHistoryAsync();
    }
}
