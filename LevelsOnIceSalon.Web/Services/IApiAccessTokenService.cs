using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;

namespace LevelsOnIceSalon.Web.Services;

public interface IApiAccessTokenService
{
    Task<ApiAccessTokenResult> TryCreateAccessTokenAsync(CreateAccessTokenRequest request, CancellationToken cancellationToken);
}
