using System.Net;
using System.Text.Json;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Tests.Infrastructure;

namespace LevelsOnIceSalon.Web.Tests.PublicApi;

public sealed class ServicesApiTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient client;

    public ServicesApiTests(IntegrationTestWebApplicationFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsPagedServices()
    {
        var response = await client.GetAsync("/api/v1/services?page=1&pageSize=3");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("1.0", string.Join(",", response.Headers.GetValues("api-supported-versions")));

        var page = await response.Content.ReadFromJsonAsync<PagedResponse<ServiceSummaryResponse>>(JsonOptions);

        Assert.NotNull(page);
        Assert.Equal(1, page!.Page);
        Assert.Equal(3, page.PageSize);
        Assert.Equal(3, page.Items.Count);
        Assert.True(page.TotalCount >= page.Items.Count);
    }

    [Fact]
    public async Task GetAll_AppliesFilteringAndSorting()
    {
        var response = await client.GetAsync("/api/v1/services?featured=true&sort=price&direction=desc&page=1&pageSize=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var page = await response.Content.ReadFromJsonAsync<PagedResponse<ServiceSummaryResponse>>(JsonOptions);

        Assert.NotNull(page);
        Assert.NotEmpty(page!.Items);
        Assert.All(page.Items, item => Assert.True(item.IsFeatured));

        var prices = page.Items
            .Where(item => item.Price.HasValue)
            .Select(item => item.Price!.Value)
            .ToList();

        Assert.Equal(prices.OrderByDescending(value => value).ToList(), prices);
    }

    [Fact]
    public async Task GetBySlug_ReturnsServiceDetail()
    {
        var list = await client.GetFromJsonAsync<PagedResponse<ServiceSummaryResponse>>("/api/v1/services?page=1&pageSize=1", JsonOptions);
        Assert.NotNull(list);
        Assert.NotEmpty(list!.Items);

        var slug = list.Items[0].Slug;
        var response = await client.GetAsync($"/api/v1/services/{slug}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var service = await response.Content.ReadFromJsonAsync<ServiceDetailResponse>(JsonOptions);

        Assert.NotNull(service);
        Assert.Equal(slug, service!.Slug);
        Assert.NotNull(service.Category);
    }
}
