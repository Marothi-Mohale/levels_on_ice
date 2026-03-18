using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using LevelsOnIceSalon.Web.Extensions;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/services")]
public sealed class ServicesController(IPublicCatalogApiService publicCatalogApiService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ServiceSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<ServiceSummaryResponse>>> GetAll(
        [FromQuery] GetServicesRequest request,
        CancellationToken cancellationToken)
    {
        var response = await publicCatalogApiService.GetServicesAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ServiceDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceDetailResponse>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var service = await publicCatalogApiService.GetServiceBySlugAsync(slug, cancellationToken);
        return service is null
            ? this.ProblemResponse(StatusCodes.Status404NotFound, "Service not found.", $"No service exists for slug '{slug}'.")
            : Ok(service);
    }
}
