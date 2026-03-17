using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class OpeningHourConfiguration : IEntityTypeConfiguration<OpeningHour>
{
    public void Configure(EntityTypeBuilder<OpeningHour> builder)
    {
        builder.ToTable("OpeningHours");

        builder.Property(x => x.Notes).HasMaxLength(120);

        builder.HasIndex(x => x.DayOfWeek).IsUnique();
    }
}
