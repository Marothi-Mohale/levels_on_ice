using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class ServicesPageService(ApplicationDbContext dbContext) : IServicesPageService
{
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
                    .Select(MapServiceCard)
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
            BannerTitle = "Premium beauty services, styled for everyday confidence and standout occasions.",
            BannerCopy = "Browse the Levels On Ice Salon service menu by category, discover featured looks, and book the treatment that matches your next beauty moment.",
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Book An Appointment",
                Url = "/book-appointment",
                SupportingText = "Move from browsing to booking with one clear action."
            },
            FeaturedServices = featuredServices,
            Categories = categorySections
        };
    }

    private static ServiceCardViewModel MapServiceCard(Service service)
    {
        return new ServiceCardViewModel
        {
            Name = service.Name,
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
}
