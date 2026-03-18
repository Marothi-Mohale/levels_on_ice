using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LevelsOnIceSalon.Web.OpenApi;

public sealed class JsonOnlyResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses.Values)
        {
            if (response.Content.Count == 0)
            {
                continue;
            }

            var contentEntries = response.Content.ToList();
            response.Content.Clear();

            foreach (var entry in contentEntries)
            {
                var targetContentType = IsProblemSchema(entry.Value.Schema)
                    ? "application/problem+json"
                    : "application/json";

                response.Content[targetContentType] = entry.Value;
            }
        }
    }

    private static bool IsProblemSchema(OpenApiSchema? schema)
    {
        var referenceId = schema?.Reference?.Id;
        return string.Equals(referenceId, "Microsoft.AspNetCore.Mvc.ProblemDetails", StringComparison.Ordinal)
            || string.Equals(referenceId, "Microsoft.AspNetCore.Mvc.ValidationProblemDetails", StringComparison.Ordinal);
    }
}
