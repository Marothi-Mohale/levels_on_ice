using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public sealed class PublicCatalogApiService(ApplicationDbContext dbContext) : IPublicCatalogApiService
{
    public async Task<IReadOnlyList<ServiceCategorySummaryResponse>> GetServiceCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => category.IsActive && !category.IsDeleted)
            .OrderBy(category => category.DisplayOrder)
            .ThenBy(category => category.Name)
            .Select(category => new ServiceCategorySummaryResponse(
                category.Id,
                category.Name,
                category.Slug,
                category.Description,
                category.DisplayOrder,
                category.Services.Count(service => service.IsActive && !service.IsDeleted)))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceCategoryDetailResponse?> GetServiceCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var category = await QueryCategories()
            .FirstOrDefaultAsync(category => category.Slug == slug, cancellationToken);

        return category is null ? null : MapCategoryDetail(category);
    }

    public async Task<PagedResponse<ServiceSummaryResponse>> GetServicesAsync(GetServicesRequest request, CancellationToken cancellationToken = default)
    {
        var query = QueryServices();

        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            var categorySlug = request.Category.Trim();
            query = query.Where(service => service.ServiceCategory != null && service.ServiceCategory.Slug == categorySlug);
        }

        if (request.Featured.HasValue)
        {
            query = query.Where(service => service.IsFeatured == request.Featured.Value);
        }

        if (request.BookableOnline.HasValue)
        {
            query = query.Where(service => service.IsBookableOnline == request.BookableOnline.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(service =>
                service.Name.Contains(search)
                || (service.ShortDescription != null && service.ShortDescription.Contains(search))
                || (service.FullDescription != null && service.FullDescription.Contains(search)));
        }

        query = ApplySorting(query, request.Sort, request.Direction);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PagedResponse<ServiceSummaryResponse>(
            items.Select(MapServiceSummary).ToList(),
            request.Page,
            request.PageSize,
            totalCount,
            totalPages);
    }

    public async Task<ServiceDetailResponse?> GetServiceBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var service = await QueryServices()
            .FirstOrDefaultAsync(service => service.Slug == slug, cancellationToken);

        return service is null ? null : MapServiceDetail(service);
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

    private IQueryable<LevelsOnIceSalon.Domain.Entities.Service> QueryServices()
    {
        return dbContext.Services
            .AsNoTracking()
            .Where(service => service.IsActive && !service.IsDeleted)
            .Include(service => service.ServiceCategory);
    }

    private static IQueryable<LevelsOnIceSalon.Domain.Entities.Service> ApplySorting(
        IQueryable<LevelsOnIceSalon.Domain.Entities.Service> query,
        string sort,
        string direction)
    {
        var descending = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);

        return sort.ToLowerInvariant() switch
        {
            "name" => descending
                ? query.OrderByDescending(service => service.Name).ThenBy(service => service.Id)
                : query.OrderBy(service => service.Name).ThenBy(service => service.Id),
            "price" => descending
                ? query.OrderByDescending(service => service.Price.HasValue ? (double?)service.Price.Value : null).ThenBy(service => service.DisplayOrder)
                : query.OrderBy(service => service.Price.HasValue ? (double?)service.Price.Value : null).ThenBy(service => service.DisplayOrder),
            "durationminutes" => descending
                ? query.OrderByDescending(service => service.DurationMinutes).ThenBy(service => service.DisplayOrder)
                : query.OrderBy(service => service.DurationMinutes).ThenBy(service => service.DisplayOrder),
            _ => descending
                ? query.OrderByDescending(service => service.DisplayOrder).ThenBy(service => service.Name)
                : query.OrderBy(service => service.DisplayOrder).ThenBy(service => service.Name)
        };
    }

    private static ServiceCategoryDetailResponse MapCategoryDetail(LevelsOnIceSalon.Domain.Entities.ServiceCategory category)
    {
        var services = category.Services
            .OrderBy(service => service.DisplayOrder)
            .ThenBy(service => service.Name)
            .Select(MapServiceSummary)
            .ToList();

        return new ServiceCategoryDetailResponse(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.DisplayOrder,
            services.Count,
            services);
    }

    private static ServiceSummaryResponse MapServiceSummary(LevelsOnIceSalon.Domain.Entities.Service service)
    {
        return new ServiceSummaryResponse(
            service.Id,
            service.Name,
            service.Slug,
            service.ShortDescription,
            service.Price,
            service.PricingType.ToString(),
            service.DurationMinutes,
            service.IsFeatured,
            service.IsBookableOnline,
            service.DisplayOrder,
            MapCategoryReference(service.ServiceCategory));
    }

    private static ServiceDetailResponse MapServiceDetail(LevelsOnIceSalon.Domain.Entities.Service service)
    {
        return new ServiceDetailResponse(
            service.Id,
            service.Name,
            service.Slug,
            service.ShortDescription,
            service.FullDescription,
            service.Price,
            service.PricingType.ToString(),
            service.DurationMinutes,
            service.IsFeatured,
            service.IsBookableOnline,
            service.DisplayOrder,
            MapCategoryReference(service.ServiceCategory));
    }

    private static ServiceCategoryReferenceResponse? MapCategoryReference(LevelsOnIceSalon.Domain.Entities.ServiceCategory? category)
    {
        return category is null
            ? null
            : new ServiceCategoryReferenceResponse(category.Id, category.Name, category.Slug);
    }
}
