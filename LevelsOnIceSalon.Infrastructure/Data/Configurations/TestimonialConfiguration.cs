using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.ToTable("Testimonials");

        builder.Property(x => x.CustomerName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Quote).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ServiceName).HasMaxLength(120);
        builder.Property(x => x.SourceUrl).HasMaxLength(400);
    }
}
