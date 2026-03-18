using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LevelsOnIceSalon.Web.Observability;

public sealed class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("The service process is running."));
    }
}
