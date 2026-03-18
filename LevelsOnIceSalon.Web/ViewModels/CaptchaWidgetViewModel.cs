namespace LevelsOnIceSalon.Web.ViewModels;

public class CaptchaWidgetViewModel
{
    public bool Enabled { get; set; }

    public string Provider { get; set; } = string.Empty;

    public string SiteKey { get; set; } = string.Empty;

    public string ScriptSource { get; set; } = string.Empty;
}
