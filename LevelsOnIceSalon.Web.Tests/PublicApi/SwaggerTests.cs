using System.Net;
using System.Text.Json;
using LevelsOnIceSalon.Web.Tests.Infrastructure;

namespace LevelsOnIceSalon.Web.Tests.PublicApi;

public sealed class SwaggerTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly HttpClient client;

    public SwaggerTests(IntegrationTestWebApplicationFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task SwaggerJson_ContainsClientGenerationMetadata()
    {
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var swaggerJson = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(swaggerJson);
        var root = document.RootElement;

        Assert.Contains("/api/v1/service-categories", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("/api/v1/services", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("Levels On Ice Salon Public API", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("Levels On Ice Salon Engineering", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("Catalog / Services", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("Catalog / Service Categories", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("Services_GetAll_v1", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("ServiceCategories_GetAll_v1", swaggerJson, StringComparison.Ordinal);
        Assert.DoesNotContain("\"/about\"", swaggerJson, StringComparison.Ordinal);
        Assert.DoesNotContain("\"/admin/login\"", swaggerJson, StringComparison.Ordinal);
        Assert.Equal("v1", root.GetProperty("info").GetProperty("version").GetString());
    }
}
