using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Starting SignalR Client...");

var hubConnection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5142/heartbeathub") // Ensure this matches your Server's port
    .WithAutomaticReconnect()
    .AddMessagePackProtocol()
    .Build();

hubConnection.Reconnecting += error => { Console.WriteLine("Reconnecting..."); return Task.CompletedTask; };
hubConnection.Reconnected += id => { Console.WriteLine("Reconnected!"); return Task.CompletedTask; };

hubConnection.On<string, DateTime>("ReceiveHeartbeat", (clientName, time) =>
{
    Console.WriteLine($"[Heartbeat] Received from {clientName} at {time:HH:mm:ss}");
});
hubConnection.On<string>("JobUpdate", message => Console.WriteLine($"[Live Status] {message}"));

try
{
    await hubConnection.StartAsync();
    Console.WriteLine("Connected using MessagePack!");
    string ClientName = $"ConsoleClient-{hubConnection.ConnectionId}";
    while (true)
    {

        if (hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.InvokeAsync("SendHeartbeat", ClientName);
        }
        await Task.Delay(2000);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}