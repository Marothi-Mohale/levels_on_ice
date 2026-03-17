using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface IServicesPageService
{
    Task<ServicesPageViewModel> GetServicesPageAsync(CancellationToken cancellationToken = default);
}
