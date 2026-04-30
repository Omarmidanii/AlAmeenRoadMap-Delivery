using FundamentalsLab.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IGreetingService, GreetingService>();

var app = builder.Build();

// app.UseHttpsRedirection(); // Commented out for local Docker testing without certs

app.MapGet("/api/greet", async (IGreetingService greetingService) =>
{
    var message = await greetingService.GetGreetingAsync();
    return Results.Ok(new { Message = message });
});

app.Run();