using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class AppointmentRequestConfiguration : IEntityTypeConfiguration<AppointmentRequest>
{
    public void Configure(EntityTypeBuilder<AppointmentRequest> builder)
    {
        builder.ToTable("AppointmentRequests");

        builder.Property(x => x.FullName).HasMaxLength(160).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(180);
        builder.Property(x => x.PreferredStylistName).HasMaxLength(120);
        builder.Property(x => x.Occasion).HasMaxLength(120);
        builder.Property(x => x.Notes).HasMaxLength(1500);
        builder.Property(x => x.InspirationReference).HasMaxLength(1500);
        builder.Property(x => x.AdminNotes).HasMaxLength(1500);

        builder.HasOne(x => x.Service)
            .WithMany(x => x.AppointmentRequests)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.TeamMember)
            .WithMany(x => x.AppointmentRequests)
            .HasForeignKey(x => x.TeamMemberId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
