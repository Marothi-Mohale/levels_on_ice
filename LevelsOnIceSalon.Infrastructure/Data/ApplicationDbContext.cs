using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<GalleryImage> GalleryImages => Set<GalleryImage>();

    public DbSet<Testimonial> Testimonials => Set<Testimonial>();

    public DbSet<FAQ> Faqs => Set<FAQ>();

    public DbSet<AppointmentRequest> AppointmentRequests => Set<AppointmentRequest>();

    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

    public DbSet<OpeningHour> OpeningHours => Set<OpeningHour>();

    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    public DbSet<PromotionBanner> PromotionBanners => Set<PromotionBanner>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        ApplicationDbContextSeed.Apply(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
