using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using LevelsOnIceSalon.Web.Options;
using LevelsOnIceSalon.Web.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LevelsOnIceSalon.Web.Services;

public sealed class ApiAccessTokenService(
    IOptions<AdminAuthOptions> adminAuthOptions,
    IOptions<ApiTokenOptions> apiTokenOptions,
    IAdminMfaService adminMfaService) : IApiAccessTokenService
{
    private readonly AdminAuthOptions adminAuthOptions = adminAuthOptions.Value;
    private readonly ApiTokenOptions apiTokenOptions = apiTokenOptions.Value;

    public Task<ApiAccessTokenResult> TryCreateAccessTokenAsync(CreateAccessTokenRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!IsValidCredentials(request.Username, request.Password) || !adminMfaService.ValidateCode(request.OneTimeCode))
        {
            return Task.FromResult(new ApiAccessTokenResult
            {
                Succeeded = false
            });
        }

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(apiTokenOptions.AccessTokenLifetimeMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.Username.Trim()),
            new(JwtRegisteredClaimNames.UniqueName, request.Username.Trim()),
            new(ClaimTypes.Name, request.Username.Trim()),
            new(ClaimTypes.Role, AuthConstants.AdminRole),
            new(ClaimTypes.AuthenticationMethod, "mfa")
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiTokenOptions.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: apiTokenOptions.Issuer,
            audience: apiTokenOptions.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: credentials);

        return Task.FromResult(new ApiAccessTokenResult
        {
            Succeeded = true,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresIn = (int)Math.Round((expires - now).TotalSeconds)
        });
    }

    private bool IsValidCredentials(string username, string password)
    {
        return string.Equals(username?.Trim(), adminAuthOptions.Username, StringComparison.Ordinal)
            && string.Equals(password, adminAuthOptions.Password, StringComparison.Ordinal);
    }
}
