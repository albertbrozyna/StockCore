var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.Run();

/// <summary>Entry point marker; exposed for WebApplicationFactory in integration tests.</summary>

