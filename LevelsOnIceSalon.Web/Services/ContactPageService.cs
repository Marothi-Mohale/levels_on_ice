using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class ContactPageService(
    ApplicationDbContext dbContext,
    ICaptchaVerificationService captchaVerificationService,
    IFormInputSanitizer formInputSanitizer,
    ILogger<ContactPageService> logger) : IContactPageService
{
    private const int MinimumSubmitSeconds = 3;
    private const string DefaultAddress = "102 Main Road, Mowbray, Cape Town";
    private const string DefaultArea = "Mowbray, Cape Town";
    private const string DefaultPhone = "081 390 6634";
    private const string DefaultEmail = "levelsonicegroup@gmail.com";

    public async Task<ContactPageViewModel> BuildPageModelAsync(
        ContactFormViewModel? form = null,
        string? statusMessage = null,
        bool isSuccess = false,
        CancellationToken cancellationToken = default)
    {
        var settings = await dbContext.SiteSettings
            .AsNoTracking()
            .Where(setting => setting.Group == "Contact")
            .ToDictionaryAsync(setting => setting.Key, setting => setting.Value, cancellationToken);

        var openingHours = await dbContext.OpeningHours
            .AsNoTracking()
            .OrderBy(hour => hour.DayOfWeek)
            .ToListAsync(cancellationToken);

        var address = GetSetting(settings, "Address", DefaultAddress);
        var area = GetSetting(settings, "Area", DefaultArea);
        var phone = GetSetting(settings, "PhoneNumber", DefaultPhone);
        var email = GetSetting(settings, "Email", DefaultEmail);

        return new ContactPageViewModel
        {
            PageTitle = "Contact",
            MetaDescription = "Contact Levels On Ice Salon in Mowbray, Cape Town for premium nails, hairstyles, beauty bookings, opening hours, and salon directions.",
            BannerTitle = "Visit, call, or message us when you are ready to plan your next polished appointment.",
            BannerCopy = "Whether you already know the look or just need a little guidance first, this is the easiest way to reach us.",
            IntroTitle = "Beautiful appointments start with clear, easy contact.",
            IntroCopy = "Reach out for bookings, service advice, occasion planning, or visit details. We keep the process warm, helpful, and easy from the start.",
            AddressLine = address,
            Area = area,
            PhoneNumber = phone,
            Email = email,
            MapPlaceholderLabel = "Map & Directions",
            MapPlaceholderCopy = "Use this section for an embedded map or directions. For now, the address is ready whenever you need to find us quickly.",
            StatusMessage = statusMessage,
            IsSuccess = isSuccess,
            BookingCta = new CallToActionViewModel
            {
                Label = "Book Your Visit",
                Url = "/book-appointment",
                SupportingText = "Move from questions to a confirmed beauty plan."
            },
            ContactDetails =
            [
                new ContactDetailItemViewModel
                {
                    Label = "Phone",
                    Value = phone,
                    Url = $"tel:{phone.Replace(" ", string.Empty)}",
                    SupportingText = "Best for quick questions, booking support, or checking details before you come in."
                },
                new ContactDetailItemViewModel
                {
                    Label = "Email",
                    Value = email,
                    Url = $"mailto:{email}",
                    SupportingText = "Ideal for service questions, occasion enquiries, and anything that needs a little more detail."
                },
                new ContactDetailItemViewModel
                {
                    Label = "Location",
                    Value = address,
                    SupportingText = "Easy to find on Main Road in Mowbray for polished appointments in Cape Town."
                }
            ],
            OpeningHours = BuildOpeningHours(openingHours),
            Form = form ?? new ContactFormViewModel
            {
                FormRenderedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            Captcha = captchaVerificationService.BuildWidget()
        };
    }

    public async Task<ContactSubmissionResult> SubmitAsync(ContactFormViewModel form, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            logger.LogWarning("Rejected contact form submission because honeypot field was populated.");
            return ContactSubmissionResult.Failure("We could not process your message. Please try again.");
        }

        var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (form.FormRenderedAtUnix <= 0 || nowUnix - form.FormRenderedAtUnix < MinimumSubmitSeconds)
        {
            logger.LogWarning("Rejected contact form submission because the form was submitted too quickly.");
            return ContactSubmissionResult.Failure("Please take a moment to review your message and submit again.");
        }

        var captchaResult = await captchaVerificationService.VerifyAsync(
            form.CaptchaToken,
            remoteIpAddress: null,
            cancellationToken);
        if (!captchaResult.Success)
        {
            logger.LogWarning("Rejected contact form submission because CAPTCHA verification failed.");
            return ContactSubmissionResult.Failure(captchaResult.Message);
        }

        var fullName = formInputSanitizer.SanitizeSingleLine(form.FullName);
        var email = formInputSanitizer.SanitizeSingleLine(form.Email);
        var phoneNumber = formInputSanitizer.SanitizeSingleLine(form.PhoneNumber);
        var subject = formInputSanitizer.SanitizeSingleLine(form.Subject);
        var message = formInputSanitizer.SanitizeMultiline(form.Message);

        if (fullName is null || email is null || subject is null || message is null)
        {
            logger.LogWarning("Rejected contact form submission because sanitized required fields were empty.");
            return ContactSubmissionResult.Failure("Please review the required fields and try again.");
        }

        if (formInputSanitizer.ContainsMarkup(fullName)
            || formInputSanitizer.ContainsMarkup(subject)
            || formInputSanitizer.ContainsMarkup(message))
        {
            logger.LogWarning("Rejected contact form submission because HTML markup was detected.");
            return ContactSubmissionResult.Failure("Please submit plain text only.");
        }

        var contactMessage = new ContactMessage
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            Subject = subject,
            Message = message,
            Status = ContactMessageStatus.New
        };

        dbContext.ContactMessages.Add(contactMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Created contact message {ContactMessageId} from {Email}.",
            contactMessage.Id,
            contactMessage.Email);

        return ContactSubmissionResult.Successful("Your message is in. We will get back to you as soon as we can.");
    }

    private static IList<ContactOpeningHourViewModel> BuildOpeningHours(IList<OpeningHour> openingHours)
    {
        var today = (int)DateTime.Today.DayOfWeek;

        if (openingHours.Count == 0)
        {
            return
            [
                new ContactOpeningHourViewModel { DayLabel = "Monday", HoursLabel = "08:30 - 18:00", IsToday = today == 1 },
                new ContactOpeningHourViewModel { DayLabel = "Tuesday", HoursLabel = "08:30 - 18:00", IsToday = today == 2 },
                new ContactOpeningHourViewModel { DayLabel = "Wednesday", HoursLabel = "08:30 - 18:00", IsToday = today == 3 },
                new ContactOpeningHourViewModel { DayLabel = "Thursday", HoursLabel = "08:30 - 18:00", IsToday = today == 4 },
                new ContactOpeningHourViewModel { DayLabel = "Friday", HoursLabel = "08:30 - 18:00", IsToday = today == 5 },
                new ContactOpeningHourViewModel { DayLabel = "Saturday", HoursLabel = "08:30 - 16:00", IsToday = today == 6 },
                new ContactOpeningHourViewModel { DayLabel = "Sunday", HoursLabel = "By appointment / closed", IsToday = today == 0 }
            ];
        }

        return openingHours
            .Select(hour => new ContactOpeningHourViewModel
            {
                DayLabel = Enum.GetName(typeof(DayOfWeek), hour.DayOfWeek) ?? "Day",
                HoursLabel = hour.IsClosed
                    ? hour.Notes ?? "Closed"
                    : $"{FormatTime(hour.OpenTime)} - {FormatTime(hour.CloseTime)}",
                IsToday = today == hour.DayOfWeek
            })
            .ToList();
    }

    private static string FormatTime(TimeOnly? time) =>
        time?.ToString("HH:mm") ?? "--:--";

    private static string GetSetting(IReadOnlyDictionary<string, string> settings, string key, string fallback) =>
        settings.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : fallback;
}
