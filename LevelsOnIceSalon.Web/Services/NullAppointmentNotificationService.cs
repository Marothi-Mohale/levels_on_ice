using LevelsOnIceSalon.Domain.Entities;

namespace LevelsOnIceSalon.Web.Services;

public class NullAppointmentNotificationService : IAppointmentNotificationService
{
    public Task NotifyNewAppointmentRequestAsync(AppointmentRequest appointmentRequest, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
