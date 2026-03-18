using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using LevelsOnIceSalon.Web.Extensions;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers.Api;

/// <summary>
/// Exposes public service catalog endpoints used by frontend clients and external consumers.
/// </summary>
[ApiController]
[Route("api/v1/services")]
public sealed class ServicesController(IPublicCatalogApiService publicCatalogApiService) : ControllerBase
{
    /// <summary>
    /// Returns a paged list of active salon services.
    /// </summary>
    /// <param name="request">The query-string filters, paging options, and sort options.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>A paged collection of service summaries.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ServiceSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<ServiceSummaryResponse>>> GetAll(
        [FromQuery] GetServicesRequest request,
        CancellationToken cancellationToken)
    {
        var response = await publicCatalogApiService.GetServicesAsync(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Returns a single active service by slug.
    /// </summary>
    /// <param name="slug">The stable service slug.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>The detailed service resource.</returns>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ServiceDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceDetailResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var service = await publicCatalogApiService.GetServiceBySlugAsync(slug, cancellationToken);
        return service is null
            ? this.ProblemResponse(StatusCodes.Status404NotFound, "Service not found.", $"No service exists for slug '{slug}'.")
            : Ok(service);
    }
}
