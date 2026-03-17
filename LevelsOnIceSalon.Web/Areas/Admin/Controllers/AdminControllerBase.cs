using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
public abstract class AdminControllerBase : Controller
{
    protected const string AdminRoleName = "Admin";
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
