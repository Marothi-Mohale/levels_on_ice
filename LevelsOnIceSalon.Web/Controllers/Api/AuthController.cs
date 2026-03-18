using System.Security.Claims;
using Asp.Versioning;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using LevelsOnIceSalon.Web.Extensions;
using LevelsOnIceSalon.Web.Security;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LevelsOnIceSalon.Web.Controllers.Api;

/// <summary>
/// Exposes token issuance and authenticated principal inspection endpoints for the public API.
/// </summary>
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController(
    IApiAccessTokenService apiAccessTokenService,
    ILogger<AuthController> logger) : ControllerBase
{
    /// <summary>
    /// Exchanges configured admin credentials and an MFA code for a short-lived bearer token.
    /// </summary>
    /// <param name="request">The admin credentials and one-time authenticator code.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>A bearer access token response.</returns>
    [AllowAnonymous]
    [EnableRateLimiting("api-token")]
    [HttpPost("token")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AccessTokenResponse>> CreateToken(
        [FromBody] CreateAccessTokenRequest request,
        CancellationToken cancellationToken)
    {
        var token = await apiAccessTokenService.TryCreateAccessTokenAsync(request, cancellationToken);
        if (!token.Succeeded)
        {
            logger.LogWarning(
                "Invalid API token request for username '{Username}' from IP '{IpAddress}'.",
                request.Username?.Trim(),
                HttpContext.Connection.RemoteIpAddress?.ToString());

            return this.ProblemResponse(
                StatusCodes.Status401Unauthorized,
                "Invalid credentials.",
                "The supplied username, password, or authenticator code is incorrect.");
        }

        return Ok(new AccessTokenResponse
        {
            AccessToken = token.AccessToken,
            ExpiresIn = token.ExpiresIn
        });
    }

    /// <summary>
    /// Returns the authenticated principal represented by the supplied bearer token.
    /// </summary>
    /// <returns>The authenticated API user.</returns>
    [Authorize(AuthenticationSchemes = AuthConstants.BearerScheme, Policy = AuthConstants.ApiAdminPolicy)]
    [HttpGet("me")]
    [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<CurrentUserResponse> Me()
    {
        var roles = User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).Distinct(StringComparer.Ordinal).ToArray();
        var authenticationMethods = User.Claims
            .Where(claim => claim.Type is ClaimTypes.AuthenticationMethod or "amr")
            .Select(claim => claim.Value)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return Ok(new CurrentUserResponse
        {
            Username = User.Identity?.Name ?? string.Empty,
            AuthenticationScheme = User.Identity?.AuthenticationType ?? string.Empty,
            Roles = roles,
            AuthenticationMethods = authenticationMethods
        });
    }
}
