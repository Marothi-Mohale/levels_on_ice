using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Infrastructure.Data.Seed;
using LevelsOnIceSalon.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Data;

public static class SeedData
{
    private static readonly string[] LegacyAppTables =
    [
        "ServiceCategories",
        "Services",
        "GalleryImages",
        "Testimonials",
        "Faqs",
        "AppointmentRequests",
        "ContactMessages",
        "SiteSettings",
        "OpeningHours",
        "TeamMembers",
        "PromotionBanners"
    ];

    public static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        bool applyMigrations,
        bool seedSampleData)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");

        logger.LogInformation("Starting database initialization.");
        if (applyMigrations)
        {
            await RecoverLegacySqliteDatabaseAsync(dbContext);
            await dbContext.Database.MigrateAsync();
        }

        if (seedSampleData)
        {
            await EnsureApplicationSeedAsync(dbContext);
        }
        logger.LogInformation("Completed database initialization.");
    }

    private static async Task RecoverLegacySqliteDatabaseAsync(ApplicationDbContext dbContext)
    {
        if (!dbContext.Database.IsSqlite())
        {
            return;
        }

        var databasePath = dbContext.Database.GetDbConnection().DataSource;
        if (string.IsNullOrWhiteSpace(databasePath) || !File.Exists(databasePath))
        {
            return;
        }

        var hasMigrationHistoryTable = await HasTableAsync(dbContext, "__EFMigrationsHistory");
        var hasAppliedMigrations = hasMigrationHistoryTable && await HasMigrationHistoryRowsAsync(dbContext);

        var hasLegacyTables = false;
        foreach (var tableName in LegacyAppTables)
        {
            if (await HasTableAsync(dbContext, tableName))
            {
                hasLegacyTables = true;
                break;
            }
        }

        if (hasAppliedMigrations || !hasLegacyTables)
        {
            return;
        }

        await dbContext.Database.CloseConnectionAsync();
        SqliteConnection.ClearAllPools();

        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        BackupSqliteArtifact(databasePath, timestamp);
        BackupSqliteArtifact($"{databasePath}-wal", timestamp);
        BackupSqliteArtifact($"{databasePath}-shm", timestamp);
    }

    private static async Task<bool> HasTableAsync(ApplicationDbContext dbContext, string tableName)
    {
        var connection = dbContext.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;

        if (shouldClose)
        {
            await connection.OpenAsync();
        }

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = $name;";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "$name";
            parameter.Value = tableName;
            command.Parameters.Add(parameter);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        finally
        {
            if (shouldClose)
            {
                await connection.CloseAsync();
            }
        }
    }

    private static async Task<bool> HasMigrationHistoryRowsAsync(ApplicationDbContext dbContext)
    {
        var connection = dbContext.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;

        if (shouldClose)
        {
            await connection.OpenAsync();
        }

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM \"__EFMigrationsHistory\";";

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        finally
        {
            if (shouldClose)
            {
                await connection.CloseAsync();
            }
        }
    }

    private static void BackupSqliteArtifact(string path, string timestamp)
    {
        if (!File.Exists(path))
        {
            return;
        }

        var directory = Path.GetDirectoryName(path) ?? string.Empty;
        var fileName = Path.GetFileName(path);
        var backupPath = Path.Combine(directory, $"{fileName}.legacy-{timestamp}.bak");
        ExecuteFileMoveWithRetry(path, backupPath);
    }

    private static void ExecuteFileMoveWithRetry(string sourcePath, string destinationPath)
    {
        const int maxAttempts = 6;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                File.Move(sourcePath, destinationPath, overwrite: true);
                return;
            }
            catch (IOException) when (attempt < maxAttempts)
            {
                SqliteConnection.ClearAllPools();
                Thread.Sleep(250 * attempt);
            }
        }

        File.Move(sourcePath, destinationPath, overwrite: true);
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

        if (!await dbContext.Testimonials.AnyAsync())
        {
            dbContext.Testimonials.AddRange(SampleData.Testimonials);
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
