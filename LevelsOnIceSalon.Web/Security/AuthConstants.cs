using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LevelsOnIceSalon.Web.Security;

public static class AuthConstants
{
    public const string AdminRole = "Admin";
    public const string ApiAdminPolicy = "ApiAdmin";
    public const string CookieScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    public const string BearerScheme = JwtBearerDefaults.AuthenticationScheme;
}
