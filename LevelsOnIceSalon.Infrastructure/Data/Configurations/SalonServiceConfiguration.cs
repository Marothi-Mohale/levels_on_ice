using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class SalonServiceConfiguration : IEntityTypeConfiguration<SalonService>
{
    public void Configure(EntityTypeBuilder<SalonService> builder)
    {
        builder.ToTable("Services");

        builder.Property(x => x.Name).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(180).IsRequired();
        builder.Property(x => x.ShortDescription).HasMaxLength(300);
        builder.Property(x => x.PriceFrom).HasColumnType("numeric(10,2)");

        builder.HasIndex(x => x.Slug).IsUnique();

        builder.HasOne(x => x.ServiceCategory)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
