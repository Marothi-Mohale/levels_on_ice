using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Options;

public class CaptchaOptions
{
    public const string SectionName = "Captcha";

    public bool Enabled { get; set; }

    [RegularExpression("^(Turnstile|hCaptcha)$", ErrorMessage = "Captcha:Provider must be either Turnstile or hCaptcha.")]
    public string Provider { get; set; } = "Turnstile";

    public string SiteKey { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    [Range(0, 1)]
    public decimal MinimumScore { get; set; } = 0.5m;
}
