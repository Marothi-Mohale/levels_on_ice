using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using LevelsOnIceSalon.Web.Contracts.PublicApi;

namespace LevelsOnIceSalon.Web.Services;

public interface IPublicCatalogApiService
{
    Task<IReadOnlyList<ServiceCategorySummaryResponse>> GetServiceCategoriesAsync(CancellationToken cancellationToken = default);

    Task<ServiceCategoryDetailResponse?> GetServiceCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<PagedResponse<ServiceSummaryResponse>> GetServicesAsync(GetServicesRequest request, CancellationToken cancellationToken = default);

    Task<ServiceDetailResponse?> GetServiceBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
