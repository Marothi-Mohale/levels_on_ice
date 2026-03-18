namespace LevelsOnIceSalon.Web.ViewModels;

public class ContactPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public string BannerTitle { get; set; } = string.Empty;

    public string BannerCopy { get; set; } = string.Empty;

    public string IntroTitle { get; set; } = string.Empty;

    public string IntroCopy { get; set; } = string.Empty;

    public string AddressLine { get; set; } = string.Empty;

    public string Area { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MapPlaceholderLabel { get; set; } = string.Empty;

    public string MapPlaceholderCopy { get; set; } = string.Empty;

    public string? StatusMessage { get; set; }

    public bool IsSuccess { get; set; }

    public CallToActionViewModel BookingCta { get; set; } = new();

    public IList<ContactDetailItemViewModel> ContactDetails { get; set; } = [];

    public IList<ContactOpeningHourViewModel> OpeningHours { get; set; } = [];

    public ContactFormViewModel Form { get; set; } = new();

    public CaptchaWidgetViewModel Captcha { get; set; } = new();
}
