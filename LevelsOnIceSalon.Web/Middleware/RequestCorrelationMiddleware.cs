using LevelsOnIceSalon.Web.Extensions;

namespace LevelsOnIceSalon.Web.Middleware;

public sealed class RequestCorrelationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.GetOrCreateCorrelationId();

        context.Response.Headers[RequestCorrelationExtensions.CorrelationIdHeaderName] = correlationId;
        context.Response.Headers[RequestCorrelationExtensions.RequestIdHeaderName] = context.GetRequestId();

        await next(context);
    }
}
