using Asp.Versioning;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Extensions;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers.Api;

/// <summary>
/// Exposes public service category endpoints used by frontend clients and external consumers.
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/service-categories")]
public sealed class ServiceCategoriesController(IPublicCatalogApiService publicCatalogApiService) : ControllerBase
{
    /// <summary>
    /// Returns all active service categories as summary resources.
    /// </summary>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>The available service category summaries.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceCategorySummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<ServiceCategorySummaryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await publicCatalogApiService.GetServiceCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    /// <summary>
    /// Returns a single active service category by slug.
    /// </summary>
    /// <param name="slug">The stable category slug.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>The detailed service category resource.</returns>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ServiceCategoryDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceCategoryDetailResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var category = await publicCatalogApiService.GetServiceCategoryBySlugAsync(slug, cancellationToken);
        return category is null
            ? this.ProblemResponse(StatusCodes.Status404NotFound, "Service category not found.", $"No service category exists for slug '{slug}'.")
            : Ok(category);
    }
}
