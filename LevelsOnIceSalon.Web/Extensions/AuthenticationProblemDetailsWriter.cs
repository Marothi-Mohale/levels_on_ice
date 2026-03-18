using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LevelsOnIceSalon.Web.Extensions;

public static class AuthenticationProblemDetailsWriter
{
    public static Task WriteAsync(HttpContext httpContext, int statusCode, string title, string detail, CancellationToken cancellationToken = default)
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        return httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails),
            cancellationToken);
    }
}
