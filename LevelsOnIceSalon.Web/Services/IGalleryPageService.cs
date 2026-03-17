using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface IGalleryPageService
{
    Task<GalleryPageViewModel> GetGalleryPageAsync(CancellationToken cancellationToken = default);
}
