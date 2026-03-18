using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using LevelsOnIceSalon.Web.Security;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = AuthConstants.AdminRole)]
[AutoValidateAntiforgeryToken]
public abstract class AdminControllerBase : Controller
{
    private const string StatusMessageKey = "AdminStatusMessage";
    private const string StatusTypeKey = "AdminStatusType";

    protected void SetSuccessMessage(string message)
    {
        TempData[StatusMessageKey] = message;
        TempData[StatusTypeKey] = "success";
    }

    protected void SetInfoMessage(string message)
    {
        TempData[StatusMessageKey] = message;
        TempData[StatusTypeKey] = "info";
    }

    protected void SetErrorMessage(string message)
    {
        TempData[StatusMessageKey] = message;
        TempData[StatusTypeKey] = "error";
    }
}
