using Microsoft.Extensions.DependencyInjection;

namespace LevelsOnIceSalon.Web.Services;

public static class WebServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<ISitePageContentService, SitePageContentService>();
        services.AddScoped<IServicesPageService, ServicesPageService>();
        services.AddScoped<IGalleryPageService, GalleryPageService>();
        services.AddScoped<IBookAppointmentService, BookAppointmentService>();
        services.AddScoped<IAppointmentNotificationService, NullAppointmentNotificationService>();
        return services;
    }
}
