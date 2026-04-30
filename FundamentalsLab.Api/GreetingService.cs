namespace FundamentalsLab.Api;

public class GreetingService : IGreetingService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GreetingService> _logger;

    public GreetingService(IConfiguration configuration, ILogger<GreetingService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GetGreetingAsync()
    {
        _logger.LogInformation("GetGreetingAsync triggered at {Time}", DateTime.UtcNow);
        await Task.Delay(1500);
        var message = _configuration["LabSettings:GreetingMessage"] ?? "Default fallback message";
        _logger.LogInformation("Successfully retrieved message: {Message}", message);
        return message;
    }
}