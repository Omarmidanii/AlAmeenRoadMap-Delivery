using Microsoft.AspNetCore.SignalR;

namespace FundamentalsLab.SignalRServer;

public class HeartbeatHub : Hub
{
    public async Task SendHeartbeat(string clientName)
    {
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveHeartbeat", clientName, DateTime.UtcNow);
    }
}