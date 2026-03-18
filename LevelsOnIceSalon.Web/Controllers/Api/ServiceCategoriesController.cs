using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/service-categories")]
[Produces("application/json")]
public sealed class ServiceCategoriesController(IPublicCatalogApiService publicCatalogApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ServiceCategoryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await publicCatalogApiService.GetServiceCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ServiceCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceCategoryResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var category = await publicCatalogApiService.GetServiceCategoryBySlugAsync(slug, cancellationToken);
        return category is null ? NotFound() : Ok(category);
    }
}
