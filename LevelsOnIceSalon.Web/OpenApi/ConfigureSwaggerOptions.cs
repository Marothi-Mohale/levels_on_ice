using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LevelsOnIceSalon.Web.OpenApi;

public sealed class ConfigureSwaggerOptions(
    IApiVersionDescriptionProvider apiVersionDescriptionProvider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.CustomSchemaIds(OpenApiSchemaIdFormatter.Build);
        options.CustomOperationIds(apiDescription =>
        {
            var controller = apiDescription.ActionDescriptor.RouteValues["controller"];
            var action = apiDescription.ActionDescriptor.RouteValues["action"];
            var groupName = apiDescription.GroupName ?? "unversioned";
            return $"{controller}_{action}_{groupName}";
        });
        options.OrderActionsBy(apiDescription => $"{apiDescription.GroupName}_{apiDescription.RelativePath}");
        options.TagActionsBy(apiDescription =>
        {
            var controller = apiDescription.ActionDescriptor.RouteValues["controller"];
            return controller switch
            {
                "Services" => ["Catalog / Services"],
                "ServiceCategories" => ["Catalog / Service Categories"],
                _ => [controller ?? "API"]
            };
        });
        options.SupportNonNullableReferenceTypes();
        options.OperationFilter<JsonOnlyResponsesOperationFilter>();
        options.DocumentFilter<ApiOnlyDocumentFilter>();
        options.MapType<decimal>(() => new OpenApiSchema
        {
            Type = "number",
            Format = "decimal"
        });
        options.DocInclusionPredicate((documentName, apiDescription) =>
            string.Equals(apiDescription.GroupName, documentName, StringComparison.OrdinalIgnoreCase));

        var xmlDocumentationFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
        var xmlDocumentationPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile);
        if (File.Exists(xmlDocumentationPath))
        {
            options.IncludeXmlComments(xmlDocumentationPath, includeControllerXmlComments: true);
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Levels On Ice Salon Public API",
            Version = description.GroupName,
            Description = "Read-only catalog API for service discovery and frontend client generation. This specification is intended to support TypeScript client generation and cross-team API review.",
            Contact = new OpenApiContact
            {
                Name = "Levels On Ice Salon Engineering",
                Email = "levelsonicegroup@gmail.com",
                Url = new Uri("https://www.levelsonicesalon.co.za")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version is deprecated.";
        }

        return info;
    }
}
