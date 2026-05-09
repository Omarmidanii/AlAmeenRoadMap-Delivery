using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer.Services;

Console.WriteLine("Starting gRPC Bidirectional Chat Client...");

// 1. Establish channel and client
using var channel = GrpcChannel.ForAddress("http://localhost:5288");
var client = new Greeter.GreeterClient(channel);

// 2. Open the bidirectional stream
using var call = client.ChatStream();

Console.WriteLine("Connected! Type a message and press Enter (or type 'exit' to quit).");

// 3. Start a background task to continuously READ incoming messages
var readTask = Task.Run(async () =>
{
    try
    {
        await foreach (var message in call.ResponseStream.ReadAllAsync())
        {
            // Change color so server messages stand out
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[{message.User}]: {message.Text}");
            Console.ResetColor();
        }
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
    {
        Console.WriteLine("Stream cancelled.");
    }
});

// 4. Use the main thread to continuously WRITE outgoing messages
while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) continue;

    if (input.ToLower() == "exit")
    {
        break;
    }

    await call.RequestStream.WriteAsync(new ChatMessage
    {
        User = "ConsoleClient",
        Text = input
    });
}

// 5. Gracefully close the connection
Console.WriteLine("Disconnecting...");
await call.RequestStream.CompleteAsync(); // Tell the server we are done sending
await readTask; // Wait for the listening task to finish processing the last messages