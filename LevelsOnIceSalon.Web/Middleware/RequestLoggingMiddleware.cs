using System.Diagnostics;
using LevelsOnIceSalon.Web.Extensions;

namespace LevelsOnIceSalon.Web.Middleware;

public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsNoiseOnlyRequest(context))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.GetOrCreateCorrelationId();
        var requestId = context.GetRequestId();
        var requestPath = context.Request.Path.Value ?? "/";
        var method = context.Request.Method;

        using var scope = logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = correlationId,
            ["RequestId"] = requestId
        });

        logger.LogDebug(
            "HTTP request started {RequestMethod} {RequestPath} CorrelationId={CorrelationId} RequestId={RequestId}",
            method,
            requestPath,
            correlationId,
            requestId);

        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            logger.LogError(
                exception,
                "HTTP request failed {RequestMethod} {RequestPath} after {ElapsedMs}ms CorrelationId={CorrelationId} RequestId={RequestId}",
                method,
                requestPath,
                stopwatch.Elapsed.TotalMilliseconds,
                correlationId,
                requestId);

            throw;
        }

        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;
        var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
        var endpointName = context.GetEndpoint()?.DisplayName;
        var endpointSuffix = string.IsNullOrWhiteSpace(endpointName) ? string.Empty : $" for {endpointName}";

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                "HTTP request completed {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMs}ms{EndpointSuffix} CorrelationId={CorrelationId} RequestId={RequestId}",
                method,
                requestPath,
                statusCode,
                elapsedMs,
                endpointSuffix,
                correlationId,
                requestId);
        }
        else if (statusCode >= StatusCodes.Status400BadRequest)
        {
            logger.LogWarning(
                "HTTP request completed {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMs}ms{EndpointSuffix} CorrelationId={CorrelationId} RequestId={RequestId}",
                method,
                requestPath,
                statusCode,
                elapsedMs,
                endpointSuffix,
                correlationId,
                requestId);
        }
        else
        {
            logger.LogInformation(
                "HTTP request completed {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMs}ms{EndpointSuffix} CorrelationId={CorrelationId} RequestId={RequestId}",
                method,
                requestPath,
                statusCode,
                elapsedMs,
                endpointSuffix,
                correlationId,
                requestId);
        }
    }

    private static bool IsNoiseOnlyRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/_framework", StringComparison.OrdinalIgnoreCase);
    }
}
