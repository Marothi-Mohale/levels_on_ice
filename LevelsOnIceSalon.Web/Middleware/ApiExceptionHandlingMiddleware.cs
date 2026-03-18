using LevelsOnIceSalon.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Middleware;

public sealed class ApiExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ApiExceptionHandlingMiddleware> logger,
    IWebHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation(
                "API request was cancelled {RequestMethod} {RequestPath} CorrelationId={CorrelationId} RequestId={RequestId}",
                context.Request.Method,
                context.Request.Path,
                context.GetOrCreateCorrelationId(),
                context.GetRequestId());
        }
        catch (Exception exception)
        {
            if (!IsApiRequest(context) || context.Response.HasStarted)
            {
                throw;
            }

            logger.LogError(
                exception,
                "Unhandled API exception {RequestMethod} {RequestPath} CorrelationId={CorrelationId} RequestId={RequestId}",
                context.Request.Method,
                context.Request.Path,
                context.GetOrCreateCorrelationId(),
                context.GetRequestId());

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = environment.IsDevelopment()
                    ? exception.Message
                    : "The server encountered an unexpected condition.",
                Instance = context.Request.Path
            };

            problemDetails.AddRequestCorrelation(context);

            await context.Response.WriteAsJsonAsync(
                problemDetails,
                options: null,
                contentType: "application/problem+json",
                cancellationToken: context.RequestAborted);
        }
    }

    private static bool IsApiRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
    }
}
