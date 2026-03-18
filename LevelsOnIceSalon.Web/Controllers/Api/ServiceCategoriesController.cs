using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Extensions;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/service-categories")]
public sealed class ServiceCategoriesController(IPublicCatalogApiService publicCatalogApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceCategorySummaryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ServiceCategorySummaryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await publicCatalogApiService.GetServiceCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ServiceCategoryDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceCategoryDetailResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var category = await publicCatalogApiService.GetServiceCategoryBySlugAsync(slug, cancellationToken);
        return category is null
            ? this.ProblemResponse(StatusCodes.Status404NotFound, "Service category not found.", $"No service category exists for slug '{slug}'.")
            : Ok(category);
    }
}
