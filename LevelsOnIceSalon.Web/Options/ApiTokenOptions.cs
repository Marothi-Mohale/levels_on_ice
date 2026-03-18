using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LevelsOnIceSalon.Web.Options;

public sealed class ApiTokenOptions : IValidatableObject
{
    public const string SectionName = "ApiTokens";

    [Required]
    public string SigningKey { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = "LevelsOnIceSalon";

    [Required]
    public string Audience { get; set; } = "LevelsOnIceSalon.Api";

    [Range(5, 1440)]
    public int AccessTokenLifetimeMinutes { get; set; } = 60;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Encoding.UTF8.GetByteCount(SigningKey) < 32)
        {
            yield return new ValidationResult(
                "ApiTokens:SigningKey must be at least 32 bytes.",
                [nameof(SigningKey)]);
        }
    }
}
