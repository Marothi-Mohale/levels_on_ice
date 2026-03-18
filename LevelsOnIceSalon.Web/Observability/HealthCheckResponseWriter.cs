using System.Text.Json;
using LevelsOnIceSalon.Web.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LevelsOnIceSalon.Web.Observability;

public static class HealthCheckResponseWriter
{
    public static async Task WriteJsonAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            totalDurationMs = report.TotalDuration.TotalMilliseconds,
            requestId = context.GetRequestId(),
            correlationId = context.GetOrCreateCorrelationId(),
            checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    status = entry.Value.Status.ToString(),
                    durationMs = entry.Value.Duration.TotalMilliseconds,
                    description = entry.Value.Description,
                    tags = entry.Value.Tags
                })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
