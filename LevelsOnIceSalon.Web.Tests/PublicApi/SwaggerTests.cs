using System.Net;
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
    public async Task SwaggerJson_ListsApiEndpoints()
    {
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var swaggerJson = await response.Content.ReadAsStringAsync();

        Assert.Contains("/api/v1/service-categories", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("/api/v1/services", swaggerJson, StringComparison.Ordinal);
        Assert.Contains("Levels On Ice Salon API", swaggerJson, StringComparison.Ordinal);
    }
}
