using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {
        builder.ToTable("Faqs");

        builder.Property(x => x.Question).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Answer).HasMaxLength(2500).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(80);
    }
}
