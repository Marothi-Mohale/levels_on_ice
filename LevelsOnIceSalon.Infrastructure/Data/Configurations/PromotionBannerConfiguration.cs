using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class PromotionBannerConfiguration : IEntityTypeConfiguration<PromotionBanner>
{
    public void Configure(EntityTypeBuilder<PromotionBanner> builder)
    {
        builder.ToTable("PromotionBanners");

        builder.Property(x => x.Title).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Subtitle).HasMaxLength(400);
        builder.Property(x => x.CallToActionText).HasMaxLength(120);
        builder.Property(x => x.CallToActionUrl).HasMaxLength(300);
    }
}
