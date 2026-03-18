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
    public async Task GetAll_ReturnsSeededCategoriesWithServices()
    {
        var response = await client.GetAsync("/api/v1/service-categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var categories = await response.Content.ReadFromJsonAsync<List<ServiceCategoryResponse>>(JsonOptions);

        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.All(categories, category => Assert.NotEmpty(category.Services));
    }

    [Fact]
    public async Task GetBySlug_ReturnsRequestedCategory()
    {
        var categories = await client.GetFromJsonAsync<List<ServiceCategoryResponse>>("/api/v1/service-categories", JsonOptions);
        Assert.NotNull(categories);

        var expectedCategory = categories![0];

        var response = await client.GetAsync($"/api/v1/service-categories/{expectedCategory.Slug}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var category = await response.Content.ReadFromJsonAsync<ServiceCategoryResponse>(JsonOptions);

        Assert.NotNull(category);
        Assert.Equal(expectedCategory.Id, category!.Id);
        Assert.Equal(expectedCategory.Name, category.Name);
        Assert.NotEmpty(category.Services);
    }

    [Fact]
    public async Task GetBySlug_ForMissingCategory_ReturnsNotFound()
    {
        var response = await client.GetAsync("/api/v1/service-categories/not-a-real-category");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
