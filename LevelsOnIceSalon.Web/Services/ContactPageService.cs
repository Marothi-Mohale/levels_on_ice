using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class ContactPageService(ApplicationDbContext dbContext) : IContactPageService
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
            BannerTitle = "Visit, call, or message Levels On Ice Salon in Mowbray, Cape Town.",
            BannerCopy = "Whether you're ready to book, need help choosing a service, or want to confirm salon details before your visit, this is the easiest place to reach us.",
            IntroTitle = "A polished beauty experience starts with clear, easy contact.",
            IntroCopy = "Levels On Ice Salon is designed for women who want stylish results, a welcoming atmosphere, and beauty appointments that feel worth the trip. Reach out for bookings, service questions, or visit details in Cape Town.",
            AddressLine = address,
            Area = area,
            PhoneNumber = phone,
            Email = email,
            MapPlaceholderLabel = "Map & Directions",
            MapPlaceholderCopy = "Use this section later for an embedded map, pin drop, or driving directions. For now, the address is ready for local search visibility and contact actions.",
            StatusMessage = statusMessage,
            IsSuccess = isSuccess,
            BookingCta = new CallToActionViewModel
            {
                Label = "Book Appointment",
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
                    SupportingText = "Call for quick salon questions, directions, or booking support."
                },
                new ContactDetailItemViewModel
                {
                    Label = "Email",
                    Value = email,
                    Url = $"mailto:{email}",
                    SupportingText = "Best for service questions, bridal enquiries, and detailed requests."
                },
                new ContactDetailItemViewModel
                {
                    Label = "Location",
                    Value = address,
                    SupportingText = "Located on Main Road in Mowbray for convenient Cape Town salon visits."
                }
            ],
            OpeningHours = BuildOpeningHours(openingHours),
            Form = form ?? new ContactFormViewModel
            {
                FormRenderedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }
        };
    }

    public async Task<ContactSubmissionResult> SubmitAsync(ContactFormViewModel form, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            return ContactSubmissionResult.Failure("We could not process your message. Please try again.");
        }

        var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (form.FormRenderedAtUnix <= 0 || nowUnix - form.FormRenderedAtUnix < MinimumSubmitSeconds)
        {
            return ContactSubmissionResult.Failure("Please take a moment to review your message and submit again.");
        }

        var contactMessage = new ContactMessage
        {
            FullName = Normalize(form.FullName)!,
            Email = Normalize(form.Email)!,
            PhoneNumber = Normalize(form.PhoneNumber),
            Subject = Normalize(form.Subject)!,
            Message = Normalize(form.Message)!,
            Status = ContactMessageStatus.New
        };

        dbContext.ContactMessages.Add(contactMessage);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ContactSubmissionResult.Successful("Your message has been sent. The salon will get back to you as soon as possible.");
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

    private static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}
