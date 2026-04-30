namespace FundamentalsLab.Api;

public interface IGreetingService
{
    Task<string> GetGreetingAsync();
}