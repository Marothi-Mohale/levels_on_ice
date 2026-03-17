using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.ToTable("ContactMessages");

        builder.Property(x => x.FullName).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(180).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(40);
        builder.Property(x => x.Subject).HasMaxLength(160).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(2500).IsRequired();
        builder.Property(x => x.AdminReplyNotes).HasMaxLength(1500);
    }
}
