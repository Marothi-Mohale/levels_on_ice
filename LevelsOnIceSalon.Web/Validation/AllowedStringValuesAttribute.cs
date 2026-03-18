using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class AllowedStringValuesAttribute(params string[] allowedValues) : ValidationAttribute
{
    private readonly HashSet<string> normalizedAllowedValues = new(allowedValues, StringComparer.OrdinalIgnoreCase);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is string textValue && normalizedAllowedValues.Contains(textValue))
        {
            return ValidationResult.Success;
        }

        var memberName = validationContext.MemberName is null ? [] : new[] { validationContext.MemberName };
        var errorMessage = ErrorMessage ?? $"{validationContext.MemberName} must be one of: {string.Join(", ", normalizedAllowedValues.OrderBy(value => value))}.";

        return new ValidationResult(errorMessage, memberName);
    }
}
