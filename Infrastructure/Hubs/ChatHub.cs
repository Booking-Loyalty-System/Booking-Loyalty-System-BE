using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinSession(string sessionId)
        {
            Console.WriteLine($"====================================");
            Console.WriteLine($"[KHACH HANG JOIN] ConnectionId: {Context.ConnectionId} -> Group: '{sessionId}'");
            Console.WriteLine($"====================================");
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

        public async Task LeaveSession(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        }

        // New admin methods
        public async Task TakeOverSession(Guid sessionId)
        {
            var userIdClaim = Context.User?.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return;

            if (!Guid.TryParse(userIdClaim, out var userId)) return;

            // Signal to clients in that session that a staff has taken over
            await Clients.Group(sessionId.ToString()).SendAsync("SessionTakenOver", new { SessionId = sessionId, StaffUserId = userId });
        }

        public async Task CloseSession(Guid sessionId)
        {
            // Notify group that session is closed
            await Clients.Group(sessionId.ToString()).SendAsync("SessionClosed", new { SessionId = sessionId });
        }
    }
}
