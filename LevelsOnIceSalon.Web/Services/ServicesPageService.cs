using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class ServicesPageService(ApplicationDbContext dbContext, IImageMetadataService imageMetadataService) : IServicesPageService
{
    private static readonly IReadOnlyDictionary<string, string> ServiceImageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["gloss-theory-gel-set"] = "/images/salon/nails-gel-set-01.jpg",
        ["iced-out-acrylic-signature-set"] = "/images/salon/nails-acrylic-01.jpg",
        ["french-girl-refill"] = "/images/salon/nails-featured-03.jpg",
        ["silk-press-sway-finish"] = "/images/salon/hair-silk-press.jpg",
        ["soft-glam-curls-styling"] = "/images/salon/hair-curls-01.jpg",
        ["knotless-braids-mid-back"] = "/images/salon/hair-braids-knotless-01.jpg",
        ["boho-knotless-luxe"] = "/images/salon/hair-braids-boho-01.jpg",
        ["sleek-cornrow-design"] = "/images/salon/hair-cornrows-01.jpg",
        ["brow-clean-up-tint"] = "/images/salon/nails-detail-01.jpg",
        ["soft-glam-face-beat-add-on"] = "/images/salon/hair-featured-02.jpg",
        ["lash-flick-finish"] = "/images/salon/hair-featured-03.jpg",
        ["bridal-preview-styling-session"] = "/images/salon/hair-bridal-01.jpg",
        ["wedding-morning-glam-hair"] = "/images/salon/hair-bridal-02.jpg",
        ["matric-dance-signature-glam"] = "/images/salon/hair-curls-02.jpg"
    };

    public async Task<ServicesPageViewModel> GetServicesPageAsync(CancellationToken cancellationToken = default)
    {
        var categories = await dbContext.ServiceCategories
            .AsNoTracking()
            .Where(category => category.IsActive && !category.IsDeleted)
            .Include(category => category.Services.Where(service => service.IsActive && !service.IsDeleted))
            .OrderBy(category => category.DisplayOrder)
            .ToListAsync(cancellationToken);

        var categorySections = categories
            .Select(category => new ServiceCategorySectionViewModel
            {
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                Services = category.Services
                    .OrderBy(service => service.DisplayOrder)
                    .Select(service => MapServiceCard(service, imageMetadataService))
                    .ToList()
            })
            .ToList();

        var featuredServices = categorySections
            .SelectMany(category => category.Services)
            .Where(service => service.IsFeatured)
            .Take(4)
            .ToList();

        return new ServicesPageViewModel
        {
            PageTitle = "Services",
            NavigationTitle = "Services",
            BannerTitle = "A service menu designed for polished days, styled nights, and every version of you in between.",
            BannerCopy = "Browse nails, hair, protective styling, and beauty services shaped with a modern eye and a refined finish, then book the one that fits your next moment.",
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Book Your Visit",
                Url = "/book-appointment",
                SupportingText = "Move from browsing to booking with one clear action."
            },
            FeaturedServices = featuredServices,
            Categories = categorySections
        };
    }

    private static ServiceCardViewModel MapServiceCard(Service service, IImageMetadataService imageMetadataService)
    {
        var imageUrl = ResolveServiceImage(service);
        var imageMetadata = imageMetadataService.GetMetadata(imageUrl);

        return new ServiceCardViewModel
        {
            Name = service.Name,
            ImageUrl = imageUrl,
            ImageWidth = imageMetadata?.Width,
            ImageHeight = imageMetadata?.Height,
            ImageAltText = ImageAltTextBuilder.ForService(service.Name),
            Summary = service.ShortDescription ?? string.Empty,
            Description = service.FullDescription,
            DurationText = service.DurationMinutes.HasValue
                ? service.DurationMinutes.Value + " mins"
                : "Timing on request",
            PriceText = service.PricingType == ServicePricingType.QuoteRequired
                ? "Quote on request"
                : service.Price.HasValue
                    ? (service.PricingType == ServicePricingType.Fixed ? "R" : "From R") + service.Price.Value.ToString("0")
                    : "Price on request",
            IsFeatured = service.IsFeatured,
            BookingUrl = "/book-appointment"
        };
    }

    private static string ResolveServiceImage(Service service)
    {
        if (ServiceImageMap.TryGetValue(service.Slug, out var imageUrl))
        {
            return imageUrl;
        }

        return service.ServiceCategoryId switch
        {
            1 => "/images/salon/nails-featured-02.jpg",
            2 => "/images/salon/hair-featured-01.jpg",
            3 => "/images/salon/hair-braids-knotless-02.jpg",
            4 => "/images/salon/salon-interior-02.jpg",
            5 => "/images/salon/hair-bridal-01.jpg",
            _ => "/images/salon/salon-interior-01.jpg"
        };
    }
}
