using LevelsOnIceSalon.Web.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LevelsOnIceSalon.Web.Services;

public static class WebServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SiteOptions>(configuration.GetSection(SiteOptions.SectionName));
        services.AddSingleton<IImageMetadataService, ImageMetadataService>();
        services.AddScoped<ISitePageContentService, SitePageContentService>();
        services.AddScoped<IServicesPageService, ServicesPageService>();
        services.AddScoped<IGalleryPageService, GalleryPageService>();
        services.AddScoped<ITestimonialsPageService, TestimonialsPageService>();
        services.AddScoped<IFaqsPageService, FaqsPageService>();
        services.AddScoped<IContactPageService, ContactPageService>();
        services.AddScoped<IBookAppointmentService, BookAppointmentService>();
        services.AddScoped<IAppointmentNotificationService, NullAppointmentNotificationService>();
        services.AddScoped<ISeoMetadataService, SeoMetadataService>();
        return services;
    }
}
