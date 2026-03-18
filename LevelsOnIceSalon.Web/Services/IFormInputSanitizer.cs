namespace LevelsOnIceSalon.Web.Services;

public interface IFormInputSanitizer
{
    string? SanitizeSingleLine(string? value);

    string? SanitizeMultiline(string? value);

    bool ContainsMarkup(string? value);
}
