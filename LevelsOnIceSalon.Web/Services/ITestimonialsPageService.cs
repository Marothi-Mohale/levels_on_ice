using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface ITestimonialsPageService
{
    Task<TestimonialsPageViewModel> BuildPageModelAsync(CancellationToken cancellationToken = default);
}
