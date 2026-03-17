using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface IBookAppointmentService
{
    Task<BookAppointmentPageViewModel> BuildPageModelAsync(BookAppointmentFormViewModel? form = null, string? statusMessage = null, bool isSuccess = false, CancellationToken cancellationToken = default);

    Task<AppointmentSubmissionResult> SubmitAsync(BookAppointmentFormViewModel form, CancellationToken cancellationToken = default);
}
