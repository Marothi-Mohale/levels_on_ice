using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Infrastructure.Data.Seed;
using LevelsOnIceSalon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database.IsSqlite())
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
        else
        {
            await dbContext.Database.MigrateAsync();
        }

        await EnsureApplicationSeedAsync(dbContext);
    }

    private static async Task EnsureApplicationSeedAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.ServiceCategories.AnyAsync())
        {
            dbContext.ServiceCategories.AddRange(SampleData.ServiceCategories);
        }

        if (!await dbContext.Services.AnyAsync())
        {
            dbContext.Services.AddRange(SampleData.Services);
        }

        if (!await dbContext.Faqs.AnyAsync())
        {
            dbContext.Faqs.AddRange(SampleData.Faqs);
        }

        if (!await dbContext.SiteSettings.AnyAsync(setting => setting.Group == "Contact"))
        {
            dbContext.SiteSettings.AddRange(
            [
                new SiteSetting { Key = "Address", Value = "102 Main Road, Mowbray, Cape Town", Group = "Contact" },
                new SiteSetting { Key = "Area", Value = "Mowbray, Cape Town", Group = "Contact" },
                new SiteSetting { Key = "PhoneNumber", Value = "081 390 6634", Group = "Contact" },
                new SiteSetting { Key = "Email", Value = "levelsonicegroup@gmail.com", Group = "Contact" }
            ]);
        }

        if (!await dbContext.OpeningHours.AnyAsync())
        {
            dbContext.OpeningHours.AddRange(
            [
                new OpeningHour { DayOfWeek = 1, OpenTime = new TimeOnly(8, 30), CloseTime = new TimeOnly(18, 0) },
                new OpeningHour { DayOfWeek = 2, OpenTime = new TimeOnly(8, 30), CloseTime = new TimeOnly(18, 0) },
                new OpeningHour { DayOfWeek = 3, OpenTime = new TimeOnly(8, 30), CloseTime = new TimeOnly(18, 0) },
                new OpeningHour { DayOfWeek = 4, OpenTime = new TimeOnly(8, 30), CloseTime = new TimeOnly(18, 0) },
                new OpeningHour { DayOfWeek = 5, OpenTime = new TimeOnly(8, 30), CloseTime = new TimeOnly(18, 0) },
                new OpeningHour { DayOfWeek = 6, OpenTime = new TimeOnly(8, 30), CloseTime = new TimeOnly(16, 0) },
                new OpeningHour { DayOfWeek = 0, IsClosed = true, Notes = "By appointment / closed" }
            ]);
        }

        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
