using LevelsOnIceSalon.Web.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LevelsOnIceSalon.Web.Services;

public static class WebServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICaptchaVerificationService, CaptchaVerificationService>();
        services.AddSingleton<IImageMetadataService, ImageMetadataService>();
        services.AddSingleton<IFormInputSanitizer, FormInputSanitizer>();
        services.AddSingleton<IAdminMfaService, AdminMfaService>();
        services.AddScoped<ISitePageContentService, SitePageContentService>();
        services.AddScoped<IServicesPageService, ServicesPageService>();
        services.AddScoped<IGalleryPageService, GalleryPageService>();
        services.AddScoped<ITestimonialsPageService, TestimonialsPageService>();
        services.AddScoped<IFaqsPageService, FaqsPageService>();
        services.AddScoped<IContactPageService, ContactPageService>();
        services.AddScoped<IPublicCatalogApiService, PublicCatalogApiService>();
        services.AddScoped<IBookAppointmentService, BookAppointmentService>();
        services.AddScoped<IAppointmentNotificationService, NullAppointmentNotificationService>();
        services.AddScoped<ISeoMetadataService, SeoMetadataService>();
        services.AddHostedService<CustomerDataBackupService>();
        return services;
    }
}
