using LevelsOnIceSalon.Web.Contracts.PublicApi;

namespace LevelsOnIceSalon.Web.Services;

public interface IPublicCatalogApiService
{
    Task<IReadOnlyList<ServiceCategoryResponse>> GetServiceCategoriesAsync(CancellationToken cancellationToken = default);

    Task<ServiceCategoryResponse?> GetServiceCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
