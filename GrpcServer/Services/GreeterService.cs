using Grpc.Core;
using GrpcServer.Services;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }

    public override async Task ChatStream(
        IAsyncStreamReader<ChatMessage> requestStream,
        IServerStreamWriter<ChatMessage> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("Client connected to ChatStream.");

        // We use an async foreach to continuously listen for incoming messages
        await foreach (var message in requestStream.ReadAllAsync())
        {
            _logger.LogInformation("Received from {User}: {Text}", message.User, message.Text);

            // Immediately fire a message back down the response stream
            var reply = new ChatMessage
            {
                User = "Server",
                Text = $"Echo: {message.Text}"
            };

            await responseStream.WriteAsync(reply);
        }

        _logger.LogInformation("Client disconnected from ChatStream.");
    }
}