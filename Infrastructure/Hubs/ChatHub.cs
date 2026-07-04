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
    }
}
