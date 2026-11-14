using Microsoft.AspNetCore.Mvc.Testing;

namespace Warehouse.IntegrationTests;

public sealed class HealthEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Health_returns_200()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health");

        response.IsSuccessStatusCode.ShouldBeTrue();
    }
}
