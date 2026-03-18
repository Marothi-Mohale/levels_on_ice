using System.Net;
using System.Text.Json;
using LevelsOnIceSalon.Web.Extensions;

namespace LevelsOnIceSalon.Web.Tests.Infrastructure;

public sealed class HealthCheckTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly HttpClient client;

    public HealthCheckTests(IntegrationTestWebApplicationFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task LiveHealthCheck_ReturnsHealthyJsonPayload()
    {
        var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.False(string.IsNullOrWhiteSpace(response.Headers.GetValues(RequestCorrelationExtensions.CorrelationIdHeaderName).Single()));
        Assert.False(string.IsNullOrWhiteSpace(response.Headers.GetValues(RequestCorrelationExtensions.RequestIdHeaderName).Single()));

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("Healthy", document.RootElement.GetProperty("status").GetString());
        Assert.True(document.RootElement.GetProperty("checks").TryGetProperty("self", out _));
    }

    [Fact]
    public async Task ReadyHealthCheck_ReturnsDatabaseHealth()
    {
        var response = await client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("Healthy", document.RootElement.GetProperty("status").GetString());
        Assert.True(document.RootElement.GetProperty("checks").TryGetProperty("database", out var databaseCheck));
        Assert.Equal("Healthy", databaseCheck.GetProperty("status").GetString());
    }
}
