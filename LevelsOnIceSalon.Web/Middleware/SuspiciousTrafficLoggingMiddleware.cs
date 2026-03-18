namespace LevelsOnIceSalon.Web.Middleware;

public class SuspiciousTrafficLoggingMiddleware(RequestDelegate next, ILogger<SuspiciousTrafficLoggingMiddleware> logger)
{
    private static readonly string[] SuspiciousPathFragments =
    [
        "/wp-admin",
        "/wp-login",
        "/.env",
        "/phpmyadmin",
        "/boaform",
        "/cgi-bin"
    ];

    public async Task Invoke(HttpContext context)
    {
        var requestPath = context.Request.Path.Value ?? string.Empty;
        if (SuspiciousPathFragments.Any(fragment => requestPath.Contains(fragment, StringComparison.OrdinalIgnoreCase)))
        {
            logger.LogWarning(
                "Suspicious request probe detected. Path={Path}, Method={Method}, IP={IpAddress}, UserAgent={UserAgent}",
                requestPath,
                context.Request.Method,
                context.Connection.RemoteIpAddress?.ToString(),
                context.Request.Headers.UserAgent.ToString());
        }

        await next(context);

        if (HttpMethods.IsPost(context.Request.Method)
            && requestPath is "/contact" or "/book-appointment" or "/admin/login"
            && context.Response.StatusCode >= StatusCodes.Status400BadRequest)
        {
            logger.LogWarning(
                "Failed form or auth submission recorded. Path={Path}, StatusCode={StatusCode}, IP={IpAddress}, UserAgent={UserAgent}",
                requestPath,
                context.Response.StatusCode,
                context.Connection.RemoteIpAddress?.ToString(),
                context.Request.Headers.UserAgent.ToString());
        }
    }
}
