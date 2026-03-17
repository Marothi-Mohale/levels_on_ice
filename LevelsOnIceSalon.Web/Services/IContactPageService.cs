using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface IContactPageService
{
    Task<ContactPageViewModel> BuildPageModelAsync(
        ContactFormViewModel? form = null,
        string? statusMessage = null,
        bool isSuccess = false,
        CancellationToken cancellationToken = default);

    Task<ContactSubmissionResult> SubmitAsync(ContactFormViewModel form, CancellationToken cancellationToken = default);
}
