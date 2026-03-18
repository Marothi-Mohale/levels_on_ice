using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface ICaptchaVerificationService
{
    CaptchaWidgetViewModel BuildWidget();

    Task<CaptchaVerificationResult> VerifyAsync(string? token, string? remoteIpAddress, CancellationToken cancellationToken = default);
}
