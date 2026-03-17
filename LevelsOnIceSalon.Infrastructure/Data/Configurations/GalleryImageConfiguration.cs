using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class GalleryImageConfiguration : IEntityTypeConfiguration<GalleryImage>
{
    public void Configure(EntityTypeBuilder<GalleryImage> builder)
    {
        builder.ToTable("GalleryImages");

        builder.Property(x => x.Title).HasMaxLength(160).IsRequired();
        builder.Property(x => x.ImagePath).HasMaxLength(400).IsRequired();
        builder.Property(x => x.ThumbnailPath).HasMaxLength(400);
        builder.Property(x => x.AltText).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SourceType).HasMaxLength(40);
        builder.Property(x => x.SourceUrl).HasMaxLength(400);
        builder.Property(x => x.Caption).HasMaxLength(500);

        builder.HasOne(x => x.Service)
            .WithMany(x => x.GalleryImages)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
