using Microsoft.Extensions.DependencyInjection;

namespace LevelsOnIceSalon.Web.Services;

public static class WebServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<ISitePageContentService, SitePageContentService>();
        return services;
    }
}
