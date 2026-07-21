using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs;

[Authorize]
public class BookingHub : Hub
{
    public async Task JoinBranchGroup(string branchId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, branchId);
    }
    
    public async Task LeaveBranchGroup(string branchId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, branchId);
    }
    
    public async Task JoinCustomerGroup(string customerId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer_{customerId}");
    }
}