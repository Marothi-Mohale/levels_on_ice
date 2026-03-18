using Microsoft.Net.Http.Headers;

namespace LevelsOnIceSalon.Web.Middleware;

public class SecurityHeadersMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task Invoke(HttpContext context)
    {
        var captchaProvider = configuration["Captcha:Provider"] ?? "Turnstile";
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;
            headers[HeaderNames.XContentTypeOptions] = "nosniff";
            headers["X-Frame-Options"] = "DENY";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            headers["X-Permitted-Cross-Domain-Policies"] = "none";
            headers["Permissions-Policy"] = "accelerometer=(), autoplay=(), camera=(), geolocation=(), gyroscope=(), microphone=(), payment=(), usb=()";
            headers["Cross-Origin-Opener-Policy"] = "same-origin";
            headers["Cross-Origin-Resource-Policy"] = "same-origin";

            var scriptSources = new List<string>
            {
                "'self'",
                "'unsafe-inline'",
                "https://cdn.jsdelivr.net",
                "https://code.jquery.com"
            };

            var frameSources = new List<string> { "'self'" };
            if (string.Equals(captchaProvider, "hCaptcha", StringComparison.OrdinalIgnoreCase))
            {
                scriptSources.Add("https://js.hcaptcha.com");
                scriptSources.Add("https://hcaptcha.com");
                scriptSources.Add("https://*.hcaptcha.com");
                frameSources.Add("https://hcaptcha.com");
                frameSources.Add("https://*.hcaptcha.com");
            }
            else
            {
                scriptSources.Add("https://challenges.cloudflare.com");
                frameSources.Add("https://challenges.cloudflare.com");
            }

            headers["Content-Security-Policy"] =
                "default-src 'self'; " +
                "base-uri 'self'; " +
                "form-action 'self'; " +
                "frame-ancestors 'none'; " +
                $"script-src {string.Join(' ', scriptSources)}; " +
                "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
                "img-src 'self' data: https:; " +
                "font-src 'self' https://fonts.gstatic.com; " +
                $"frame-src {string.Join(' ', frameSources)}; " +
                "connect-src 'self' https://challenges.cloudflare.com https://hcaptcha.com https://*.hcaptcha.com; " +
                "object-src 'none'; " +
                "upgrade-insecure-requests";

            return Task.CompletedTask;
        });

        await next(context);
    }
}
