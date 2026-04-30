using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace FundamentalsLab.SignalRServer;

public class JobProcessingWorker : BackgroundService
{
    private readonly ChannelReader<string> _channelReader;
    private readonly IHubContext<HeartbeatHub> _hubContext;

    public JobProcessingWorker(ChannelReader<string> channelReader, IHubContext<HeartbeatHub> hubContext)
    {
        _channelReader = channelReader;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var jobId in _channelReader.ReadAllAsync(stoppingToken))
        {
            await _hubContext.Clients.All.SendAsync("JobUpdate", $"Started processing {jobId}", stoppingToken);
            await Task.Delay(2000, stoppingToken);
            await _hubContext.Clients.All.SendAsync("JobUpdate", $"Successfully completed {jobId}", stoppingToken);
        }
    }
}