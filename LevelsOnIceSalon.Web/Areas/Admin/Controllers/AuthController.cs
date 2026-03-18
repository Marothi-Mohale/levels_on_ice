using System.Security.Claims;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using LevelsOnIceSalon.Web.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AllowAnonymous]
public class AuthController(
    IOptions<AdminAuthOptions> adminAuthOptions,
    IAdminMfaService adminMfaService,
    ILogger<AuthController> logger) : Controller
{
    private readonly AdminAuthOptions authOptions = adminAuthOptions.Value;

    [HttpGet("/admin/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToLocal(returnUrl);
        }

        return View(new AdminLoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [EnableRateLimiting("admin-login")]
    [HttpPost("/admin/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminLoginViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!IsValidCredentials(model.Username, model.Password))
        {
            logger.LogWarning(
                "Invalid admin login attempt for username '{Username}' from IP '{IpAddress}'.",
                model.Username?.Trim(),
                HttpContext.Connection.RemoteIpAddress?.ToString());
            ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
            model.StatusMessage = "Please use the configured admin login details.";
            return View(model);
        }

        if (!adminMfaService.ValidateCode(model.OneTimeCode))
        {
            logger.LogWarning(
                "Invalid admin MFA code attempt for username '{Username}' from IP '{IpAddress}'.",
                model.Username?.Trim(),
                HttpContext.Connection.RemoteIpAddress?.ToString());
            ModelState.AddModelError(string.Empty, "The username, password, or authenticator code is incorrect.");
            model.StatusMessage = "Admin access requires the configured MFA code.";
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, model.Username.Trim()),
            new(ClaimTypes.Role, AdminControllerBase.AdminRoleName),
            new("amr", "mfa")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        logger.LogInformation(
            "Admin user '{Username}' signed in from IP '{IpAddress}'.",
            model.Username.Trim(),
            HttpContext.Connection.RemoteIpAddress?.ToString());

        return RedirectToLocal(model.ReturnUrl);
    }

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("/admin/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        logger.LogInformation(
            "Admin user '{Username}' signed out.",
            User.Identity?.Name);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private bool IsValidCredentials(string username, string password)
    {
        return string.Equals(username?.Trim(), authOptions.Username, StringComparison.Ordinal)
            && string.Equals(password, authOptions.Password, StringComparison.Ordinal);
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
}
