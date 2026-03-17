using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Infrastructure.Data.Seed;

public static class ApplicationDbContextSeed
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceCategory>().HasData(SampleData.ServiceCategories);
        modelBuilder.Entity<Service>().HasData(SampleData.Services);
        modelBuilder.Entity<FAQ>().HasData(SampleData.Faqs);
    }
}
