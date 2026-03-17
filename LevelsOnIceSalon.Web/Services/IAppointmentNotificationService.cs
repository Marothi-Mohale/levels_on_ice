using LevelsOnIceSalon.Domain.Entities;

namespace LevelsOnIceSalon.Web.Services;

public interface IAppointmentNotificationService
{
    Task NotifyNewAppointmentRequestAsync(AppointmentRequest appointmentRequest, CancellationToken cancellationToken = default);
}
