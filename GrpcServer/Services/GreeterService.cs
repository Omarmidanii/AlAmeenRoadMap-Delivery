using Grpc.Core;
using GrpcServer.Data;     // Required to see AppDbContext and ChatMessageRecord
using GrpcServer.Services;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly AppDbContext _db; // 1. Add the database context field

    // 2. Inject the database context via the constructor
    public GreeterService(ILogger<GreeterService> logger, AppDbContext db) 
    {
        _logger = logger;
        _db = db;
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

        await foreach (var message in requestStream.ReadAllAsync())
        {
            _logger.LogInformation("Received from {User}: {Text}", message.User, message.Text);

            // 3. Map the gRPC message to your EF Core Entity and save it!
            var record = new ChatMessageRecord 
            { 
                User = message.User, 
                Text = message.Text 
            };
            
            _db.ChatMessages.Add(record);
            await _db.SaveChangesAsync(); // This translates to a SQL INSERT command

            var reply = new ChatMessage 
            { 
                User = "Server", 
                Text = $"Echo: {message.Text} (Saved to SQL!)" 
            };
            
            await responseStream.WriteAsync(reply);
        }
    }
}