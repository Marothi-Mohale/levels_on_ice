namespace LevelsOnIceSalon.Web.Services;

public interface IAdminMfaService
{
    bool IsRequired { get; }

    bool ValidateCode(string? code);
}
