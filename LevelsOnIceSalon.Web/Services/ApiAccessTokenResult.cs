namespace LevelsOnIceSalon.Web.Services;

public sealed class ApiAccessTokenResult
{
    public bool Succeeded { get; init; }

    public string AccessToken { get; init; } = string.Empty;

    public int ExpiresIn { get; init; }
}
