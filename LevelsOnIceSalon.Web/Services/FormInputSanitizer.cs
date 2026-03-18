using System.Text.RegularExpressions;

namespace LevelsOnIceSalon.Web.Services;

public partial class FormInputSanitizer : IFormInputSanitizer
{
    public string? SanitizeSingleLine(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var sanitized = ControlCharactersRegex().Replace(value, string.Empty);
        sanitized = MultiWhitespaceRegex().Replace(sanitized, " ").Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? null : sanitized;
    }

    public string? SanitizeMultiline(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var sanitized = value.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');
        sanitized = UnsafeControlCharactersRegex().Replace(sanitized, string.Empty);
        sanitized = MultiBlankLinesRegex().Replace(sanitized, "\n\n").Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? null : sanitized;
    }

    public bool ContainsMarkup(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return HtmlTagRegex().IsMatch(value);
    }

    [GeneratedRegex(@"[\u0000-\u001F\u007F]+", RegexOptions.Compiled)]
    private static partial Regex ControlCharactersRegex();

    [GeneratedRegex(@"[^\P{Cc}\n\t]+", RegexOptions.Compiled)]
    private static partial Regex UnsafeControlCharactersRegex();

    [GeneratedRegex(@"\s{2,}", RegexOptions.Compiled)]
    private static partial Regex MultiWhitespaceRegex();

    [GeneratedRegex(@"\n{3,}", RegexOptions.Compiled)]
    private static partial Regex MultiBlankLinesRegex();

    [GeneratedRegex(@"<[^>]+>", RegexOptions.Compiled)]
    private static partial Regex HtmlTagRegex();
}
