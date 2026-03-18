using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public sealed class PublicCatalogApiService(ApplicationDbContext dbContext) : IPublicCatalogApiService
{
    public async Task<IReadOnlyList<ServiceCategoryResponse>> GetServiceCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await QueryCategories()
            .ToListAsync(cancellationToken);

        return categories
            .Select(MapCategory)
            .ToList();
    }

    public async Task<ServiceCategoryResponse?> GetServiceCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var category = await QueryCategories()
            .FirstOrDefaultAsync(category => category.Slug == slug, cancellationToken);

        return category is null ? null : MapCategory(category);
    }

    private IQueryable<LevelsOnIceSalon.Domain.Entities.ServiceCategory> QueryCategories()
    {
        return dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => category.IsActive && !category.IsDeleted)
            .Include(category => category.Services.Where(service => service.IsActive && !service.IsDeleted))
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name);
    }

    private static ServiceCategoryResponse MapCategory(LevelsOnIceSalon.Domain.Entities.ServiceCategory category)
    {
        var services = category.Services
            .OrderBy(service => service.DisplayOrder)
            .ThenBy(service => service.Name)
            .Select(service => new ServiceSummaryResponse(
                service.Id,
                service.ServiceCategoryId,
                service.Name,
                service.Slug,
                service.ShortDescription,
                service.FullDescription,
                service.Price,
                service.PricingType.ToString(),
                service.DurationMinutes,
                service.IsFeatured,
                service.IsBookableOnline,
                service.DisplayOrder))
            .ToList();

        return new ServiceCategoryResponse(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.DisplayOrder,
            services);
    }
}
