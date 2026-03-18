using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LevelsOnIceSalon.Web.OpenApi;

public sealed class ApiOnlyDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var nonApiPaths = swaggerDoc.Paths.Keys
            .Where(path => !path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var path in nonApiPaths)
        {
            swaggerDoc.Paths.Remove(path);
        }
    }
}
