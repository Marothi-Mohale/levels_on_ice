using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LevelsOnIceSalon.Infrastructure.Data.Converters;

public sealed class NullableUtcDateTimeConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableUtcDateTimeConverter()
        : base(
            value => value.HasValue
                ? (value.Value.Kind == DateTimeKind.Utc ? value : value.Value.ToUniversalTime())
                : value,
            value => value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : value)
    {
    }
}
