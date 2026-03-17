using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LevelsOnIceSalon.Infrastructure.Data.Configurations;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.Property(x => x.FullName).HasMaxLength(140).IsRequired();
        builder.Property(x => x.ShortBio).HasMaxLength(250);
        builder.Property(x => x.FullBio).HasMaxLength(2000);
        builder.Property(x => x.PhotoPath).HasMaxLength(400);
        builder.Property(x => x.InstagramHandle).HasMaxLength(160);
        builder.Property(x => x.Specialty).HasMaxLength(160);
    }
}
