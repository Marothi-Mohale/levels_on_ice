using System.Net;
using System.Text.Json;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Tests.Infrastructure;

namespace LevelsOnIceSalon.Web.Tests.PublicApi;

public sealed class ServiceCategoriesApiTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient client;

    public ServiceCategoriesApiTests(IntegrationTestWebApplicationFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsCategorySummaries()
    {
        var response = await client.GetAsync("/api/v1/service-categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var categories = await response.Content.ReadFromJsonAsync<List<ServiceCategorySummaryResponse>>(JsonOptions);

        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.All(categories, category => Assert.True(category.ServiceCount > 0));
    }

    [Fact]
    public async Task GetBySlug_ReturnsRequestedCategoryDetail()
    {
        var categories = await client.GetFromJsonAsync<List<ServiceCategorySummaryResponse>>("/api/v1/service-categories", JsonOptions);
        Assert.NotNull(categories);

        var expectedCategory = categories![0];
        var response = await client.GetAsync($"/api/v1/service-categories/{expectedCategory.Slug}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var category = await response.Content.ReadFromJsonAsync<ServiceCategoryDetailResponse>(JsonOptions);

        Assert.NotNull(category);
        Assert.Equal(expectedCategory.Id, category!.Id);
        Assert.Equal(expectedCategory.Name, category.Name);
        Assert.True(category.ServiceCount >= category.Services.Count);
        Assert.NotEmpty(category.Services);
    }

    [Fact]
    public async Task GetBySlug_ForMissingCategory_ReturnsProblemDetails()
    {
        var response = await client.GetAsync("/api/v1/service-categories/not-a-real-category");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
