using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Extensions;

public static partial class RequestCorrelationExtensions
{
    public const string CorrelationIdHeaderName = "X-Correlation-Id";
    public const string RequestIdHeaderName = "X-Request-Id";
    private const string CorrelationIdItemKey = "__RequestCorrelationId";

    [GeneratedRegex("^[A-Za-z0-9._\\-]{1,64}$", RegexOptions.Compiled, matchTimeoutMilliseconds: 100)]
    private static partial Regex CorrelationIdPattern();

    public static string GetOrCreateCorrelationId(this HttpContext context)
    {
        if (context.Items.TryGetValue(CorrelationIdItemKey, out var value) && value is string correlationId)
        {
            return correlationId;
        }

        var headerValue = context.Request.Headers[CorrelationIdHeaderName].ToString().Trim();
        correlationId = IsSafeCorrelationId(headerValue)
            ? headerValue
            : context.TraceIdentifier;

        context.Items[CorrelationIdItemKey] = correlationId;
        return correlationId;
    }

    public static string GetRequestId(this HttpContext context) => context.TraceIdentifier;

    public static void AddRequestCorrelation(this ProblemDetails problemDetails, HttpContext context)
    {
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["correlationId"] = context.GetOrCreateCorrelationId();
    }

    private static bool IsSafeCorrelationId(string value)
    {
        return !string.IsNullOrWhiteSpace(value) && CorrelationIdPattern().IsMatch(value);
    }
}
