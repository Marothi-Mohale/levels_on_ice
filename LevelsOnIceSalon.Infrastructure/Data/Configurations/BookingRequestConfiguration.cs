using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
{
    public void Configure(EntityTypeBuilder<BookingRequest> builder)
    {
        builder.ToTable("BookingRequests");

        builder.Property(x => x.FullName).HasMaxLength(160).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(180);
        builder.Property(x => x.Status).HasMaxLength(40).IsRequired();
        builder.Property(x => x.AdminNotes).HasMaxLength(1500);

        builder.HasOne(x => x.Service)
            .WithMany(x => x.BookingRequests)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
