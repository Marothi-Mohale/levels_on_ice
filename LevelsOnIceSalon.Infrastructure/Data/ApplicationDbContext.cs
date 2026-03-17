using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();

    public DbSet<SalonService> Services => Set<SalonService>();

    public DbSet<GalleryImage> GalleryImages => Set<GalleryImage>();

    public DbSet<Testimonial> Testimonials => Set<Testimonial>();

    public DbSet<FaqItem> FaqItems => Set<FaqItem>();

    public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();

    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
