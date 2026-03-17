using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface IFaqsPageService
{
    Task<FaqsPageViewModel> BuildPageModelAsync(CancellationToken cancellationToken = default);
}
