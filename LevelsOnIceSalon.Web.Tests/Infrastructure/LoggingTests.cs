using System.Net;
using LevelsOnIceSalon.Web.Extensions;
using Microsoft.Extensions.Logging;

namespace LevelsOnIceSalon.Web.Tests.Infrastructure;

public sealed class LoggingTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory factory;
    private readonly HttpClient client;

    public LoggingTests(IntegrationTestWebApplicationFactory factory)
    {
        this.factory = factory;
        client = factory.CreateClient();
    }

    [Fact]
    public async Task RequestLogging_AddsCorrelationHeaders_AndAvoidsQueryStringLogging()
    {
        const string correlationId = "corr-logging-test-001";
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/services?page=1&pageSize=1");
        request.Headers.Add(RequestCorrelationExtensions.CorrelationIdHeaderName, correlationId);

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(correlationId, response.Headers.GetValues(RequestCorrelationExtensions.CorrelationIdHeaderName).Single());
        Assert.False(string.IsNullOrWhiteSpace(response.Headers.GetValues(RequestCorrelationExtensions.RequestIdHeaderName).Single()));

        var logs = factory.GetLogs();
        var completionLog = logs.LastOrDefault(entry =>
            entry.Category.Contains("RequestLoggingMiddleware", StringComparison.Ordinal)
            && entry.Properties.TryGetValue("CorrelationId", out var loggedCorrelationId)
            && string.Equals(loggedCorrelationId?.ToString(), correlationId, StringComparison.Ordinal)
            && entry.Message.Contains("/api/v1/services", StringComparison.Ordinal));

        Assert.NotNull(completionLog);
        Assert.Equal(LogLevel.Information, completionLog!.LogLevel);
        Assert.Equal("GET", completionLog.Properties["RequestMethod"]);
        Assert.Equal("/api/v1/services", completionLog.Properties["RequestPath"]);
        Assert.Equal(200, Convert.ToInt32(completionLog.Properties["StatusCode"]));
        Assert.DoesNotContain("page=1", completionLog.Message, StringComparison.Ordinal);
    }
}
