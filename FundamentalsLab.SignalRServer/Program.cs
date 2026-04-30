using System.Threading.Channels;
using FundamentalsLab.SignalRServer;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddMessagePackProtocol();

var jobChannel = Channel.CreateBounded<string>(new BoundedChannelOptions(100)
{
    FullMode = BoundedChannelFullMode.Wait
});
builder.Services.AddSingleton(jobChannel.Writer);
builder.Services.AddSingleton(jobChannel.Reader);
builder.Services.AddHostedService<JobProcessingWorker>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<HeartbeatHub>("/heartbeathub");

app.MapPost("/api/jobs", async (ChannelWriter<string> writer) =>
{
    var jobId = $"JOB-{Guid.NewGuid().ToString()[..4]}";
    await writer.WriteAsync(jobId);
    return Results.Ok(new { Message = $"Job {jobId} added to the queue." });
});

app.Run();