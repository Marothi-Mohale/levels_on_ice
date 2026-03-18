using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class BookAppointmentService(
    ApplicationDbContext dbContext,
    IAppointmentNotificationService appointmentNotificationService,
    ICaptchaVerificationService captchaVerificationService,
    IFormInputSanitizer formInputSanitizer,
    ILogger<BookAppointmentService> logger) : IBookAppointmentService
{
    private const int MinimumSubmitSeconds = 3;

    public async Task<BookAppointmentPageViewModel> BuildPageModelAsync(
        BookAppointmentFormViewModel? form = null,
        string? statusMessage = null,
        bool isSuccess = false,
        CancellationToken cancellationToken = default)
    {
        var services = await dbContext.Services
            .AsNoTracking()
            .Where(service => service.IsActive && !service.IsDeleted && service.IsBookableOnline)
            .OrderBy(service => service.ServiceCategory!.DisplayOrder)
            .ThenBy(service => service.DisplayOrder)
            .Select(service => new ServiceOptionViewModel
            {
                Id = service.Id,
                Name = service.Name
            })
            .ToListAsync(cancellationToken);

        return new BookAppointmentPageViewModel
        {
            PageTitle = "Book Appointment",
            BannerTitle = "Book your next appointment with the same ease and polish you expect from the final result.",
            BannerCopy = "Choose your service, share your preferred date and time, and send your request. We will follow up to confirm the details and make your visit feel seamless from the start.",
            MinimumPreferredDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd"),
            StatusMessage = statusMessage,
            IsSuccess = isSuccess,
            Services = services,
            Form = form ?? new BookAppointmentFormViewModel
            {
                FormRenderedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            Captcha = captchaVerificationService.BuildWidget()
        };
    }

    public async Task<AppointmentSubmissionResult> SubmitAsync(BookAppointmentFormViewModel form, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            logger.LogWarning("Rejected booking request because honeypot field was populated.");
            return AppointmentSubmissionResult.Failure("We could not process your request. Please try again.");
        }

        var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (form.FormRenderedAtUnix <= 0 || nowUnix - form.FormRenderedAtUnix < MinimumSubmitSeconds)
        {
            logger.LogWarning("Rejected booking request because the form was submitted too quickly.");
            return AppointmentSubmissionResult.Failure("Please take a moment to review your details and submit again.");
        }

        var captchaResult = await captchaVerificationService.VerifyAsync(
            form.CaptchaToken,
            remoteIpAddress: null,
            cancellationToken);
        if (!captchaResult.Success)
        {
            logger.LogWarning("Rejected booking request because CAPTCHA verification failed.");
            return AppointmentSubmissionResult.Failure(captchaResult.Message);
        }

        if (form.PreferredDate < DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogWarning("Rejected booking request with past preferred date {PreferredDate}.", form.PreferredDate);
            return AppointmentSubmissionResult.Failure("Please choose a preferred date that is today or later.");
        }

        var serviceExists = await dbContext.Services
            .AsNoTracking()
            .AnyAsync(service => service.Id == form.PreferredServiceId && service.IsActive && !service.IsDeleted && service.IsBookableOnline, cancellationToken);

        if (!serviceExists)
        {
            logger.LogWarning(
                "Rejected booking request for unavailable service id {ServiceId}.",
                form.PreferredServiceId);
            return AppointmentSubmissionResult.Failure("The selected service is no longer available. Please choose another service.");
        }

        var fullName = formInputSanitizer.SanitizeSingleLine(form.FullName);
        var phoneNumber = formInputSanitizer.SanitizeSingleLine(form.PhoneNumber);
        var email = formInputSanitizer.SanitizeSingleLine(form.Email);
        var notes = formInputSanitizer.SanitizeMultiline(form.Notes);

        if (fullName is null || phoneNumber is null)
        {
            logger.LogWarning("Rejected booking request because sanitized required fields were empty.");
            return AppointmentSubmissionResult.Failure("Please review the required fields and try again.");
        }

        if (formInputSanitizer.ContainsMarkup(fullName) || formInputSanitizer.ContainsMarkup(notes))
        {
            logger.LogWarning("Rejected booking request because HTML markup was detected.");
            return AppointmentSubmissionResult.Failure("Please submit plain text only.");
        }

        var appointmentRequest = new AppointmentRequest
        {
            FullName = fullName,
            PhoneNumber = phoneNumber,
            Email = email,
            ServiceId = form.PreferredServiceId,
            PreferredDate = form.PreferredDate,
            PreferredTime = form.PreferredTime,
            Notes = notes,
            Status = AppointmentRequestStatus.Pending,
            Source = AppointmentRequestSource.Website
        };

        dbContext.AppointmentRequests.Add(appointmentRequest);
        await dbContext.SaveChangesAsync(cancellationToken);
        await appointmentNotificationService.NotifyNewAppointmentRequestAsync(appointmentRequest, cancellationToken);
        logger.LogInformation(
            "Created appointment request {AppointmentRequestId} for {FullName} and service {ServiceId}.",
            appointmentRequest.Id,
            appointmentRequest.FullName,
            appointmentRequest.ServiceId);

        return AppointmentSubmissionResult.Successful("Your booking request is in. We will be in touch soon to confirm the final details.");
    }
}
