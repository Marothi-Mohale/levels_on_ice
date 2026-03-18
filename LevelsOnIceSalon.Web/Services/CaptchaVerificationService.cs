using System.Globalization;
using System.Text.Json;
using LevelsOnIceSalon.Web.Options;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LevelsOnIceSalon.Web.Services;

public class CaptchaVerificationService(
    HttpClient httpClient,
    IHttpContextAccessor httpContextAccessor,
    IOptions<CaptchaOptions> captchaOptions,
    ILogger<CaptchaVerificationService> logger) : ICaptchaVerificationService
{
    private readonly CaptchaOptions options = captchaOptions.Value;

    public CaptchaWidgetViewModel BuildWidget()
    {
        if (!options.Enabled)
        {
            return new CaptchaWidgetViewModel();
        }

        return new CaptchaWidgetViewModel
        {
            Enabled = true,
            Provider = options.Provider,
            SiteKey = options.SiteKey,
            ScriptSource = string.Equals(options.Provider, "hCaptcha", StringComparison.OrdinalIgnoreCase)
                ? "https://js.hcaptcha.com/1/api.js?render=explicit&onload=levelsOnIceCaptchaOnLoad"
                : "https://challenges.cloudflare.com/turnstile/v0/api.js?render=explicit&onload=levelsOnIceCaptchaOnLoad"
        };
    }

    public async Task<CaptchaVerificationResult> VerifyAsync(string? token, string? remoteIpAddress, CancellationToken cancellationToken = default)
    {
        if (!options.Enabled)
        {
            return CaptchaVerificationResult.Passed();
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return CaptchaVerificationResult.Failed("Please complete the verification challenge.");
        }

        var verificationPayload = new Dictionary<string, string>
        {
            ["secret"] = options.SecretKey,
            ["response"] = token.Trim()
        };

        var remoteIp = remoteIpAddress ?? httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        if (!string.IsNullOrWhiteSpace(remoteIp))
        {
            verificationPayload["remoteip"] = remoteIp;
        }

        var response = await httpClient.PostAsync(
            GetVerificationEndpoint(),
            new FormUrlEncodedContent(verificationPayload),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("CAPTCHA verification request failed with status code {StatusCode}.", (int)response.StatusCode);
            return CaptchaVerificationResult.Failed("We could not verify your submission. Please try again.");
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(responseStream, cancellationToken: cancellationToken);
        var root = document.RootElement;

        if (!root.TryGetProperty("success", out var successElement) || !successElement.GetBoolean())
        {
            logger.LogWarning(
                "CAPTCHA verification was rejected. Provider={Provider}, Errors={Errors}.",
                options.Provider,
                string.Join(", ", ReadErrorCodes(root)));
            return CaptchaVerificationResult.Failed("We could not verify your submission. Please try again.");
        }

        if (root.TryGetProperty("score", out var scoreElement)
            && scoreElement.ValueKind == JsonValueKind.Number
            && scoreElement.TryGetDecimal(out var score)
            && score < options.MinimumScore)
        {
            logger.LogWarning(
                "CAPTCHA verification score {Score} was below the minimum score {MinimumScore}.",
                score.ToString(CultureInfo.InvariantCulture),
                options.MinimumScore.ToString(CultureInfo.InvariantCulture));
            return CaptchaVerificationResult.Failed("We could not verify your submission. Please try again.");
        }

        return CaptchaVerificationResult.Passed();
    }

    private string GetVerificationEndpoint()
    {
        return string.Equals(options.Provider, "hCaptcha", StringComparison.OrdinalIgnoreCase)
            ? "https://hcaptcha.com/siteverify"
            : "https://challenges.cloudflare.com/turnstile/v0/siteverify";
    }

    private static IEnumerable<string> ReadErrorCodes(JsonElement root)
    {
        if (!root.TryGetProperty("error-codes", out var errorCodesElement)
            || errorCodesElement.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return errorCodesElement.EnumerateArray()
            .Where(element => element.ValueKind == JsonValueKind.String)
            .Select(element => element.GetString() ?? string.Empty)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToArray();
    }
}
