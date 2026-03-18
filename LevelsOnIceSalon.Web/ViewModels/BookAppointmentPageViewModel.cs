namespace LevelsOnIceSalon.Web.ViewModels;

public class BookAppointmentPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string BannerTitle { get; set; } = string.Empty;

    public string BannerCopy { get; set; } = string.Empty;

    public string MinimumPreferredDate { get; set; } = string.Empty;

    public string? StatusMessage { get; set; }

    public bool IsSuccess { get; set; }

    public IList<ServiceOptionViewModel> Services { get; set; } = [];

    public BookAppointmentFormViewModel Form { get; set; } = new();

    public CaptchaWidgetViewModel Captcha { get; set; } = new();
}
