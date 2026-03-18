using System.Net;
using System.Text.Json;
using LevelsOnIceSalon.Web.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Tests.PublicApi;

public sealed class ApiProblemDetailsTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient client;

    public ApiProblemDetailsTests(IntegrationTestWebApplicationFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetServiceBySlug_ForMissingResource_ReturnsProblemDetails()
    {
        var response = await client.GetAsync("/api/v1/services/not-a-real-service");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);

        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status404NotFound, problem!.Status);
        Assert.Equal("Service not found.", problem.Title);
    }

    [Fact]
    public async Task GetServices_WithInvalidQuery_ReturnsValidationProblemDetails()
    {
        var response = await client.GetAsync("/api/v1/services?page=0&pageSize=101&sort=unknown&direction=sideways");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(JsonOptions);

        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status400BadRequest, problem!.Status);
        Assert.Contains("page", problem.Errors.Keys);
        Assert.Contains("pageSize", problem.Errors.Keys);
        Assert.Contains("sort", problem.Errors.Keys);
        Assert.Contains("direction", problem.Errors.Keys);
    }
}
