using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LevelsOnIceSalon.Web.OpenApi;

public sealed class BearerSecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any()
            || context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() == true;

        if (hasAllowAnonymous)
        {
            return;
        }

        var hasAuthorize = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
            || context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true;

        if (!hasAuthorize)
        {
            return;
        }

        operation.Security ??= [];
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }
            ] = []
        });

        if (!operation.Responses.ContainsKey("401"))
        {
            operation.Responses["401"] = new OpenApiResponse { Description = "Unauthorized" };
        }

        if (!operation.Responses.ContainsKey("403"))
        {
            operation.Responses["403"] = new OpenApiResponse { Description = "Forbidden" };
        }
    }
}
