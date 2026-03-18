namespace LevelsOnIceSalon.Web.Services;

public sealed record CaptchaVerificationResult(bool Success, bool IsSuspicious, string Message)
{
    public static CaptchaVerificationResult Passed() => new(true, false, string.Empty);

    public static CaptchaVerificationResult Failed(string message, bool isSuspicious = true) => new(false, isSuspicious, message);
}
