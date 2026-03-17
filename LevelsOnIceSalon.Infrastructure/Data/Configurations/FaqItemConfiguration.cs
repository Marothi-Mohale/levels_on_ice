using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class FaqItemConfiguration : IEntityTypeConfiguration<FaqItem>
{
    public void Configure(EntityTypeBuilder<FaqItem> builder)
    {
        builder.ToTable("FaqItems");

        builder.Property(x => x.Question).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Answer).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(80);
    }
}
