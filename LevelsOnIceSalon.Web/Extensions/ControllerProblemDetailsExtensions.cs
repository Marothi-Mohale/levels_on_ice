using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Extensions;

public static class ControllerProblemDetailsExtensions
{
    public static ObjectResult ProblemResponse(
        this ControllerBase controller,
        int statusCode,
        string title,
        string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = controller.HttpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = controller.HttpContext.TraceIdentifier;

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }
}
