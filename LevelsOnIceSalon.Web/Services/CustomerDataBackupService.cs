using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using LevelsOnIceSalon.Web.Options;

namespace LevelsOnIceSalon.Web.Services;

public class CustomerDataBackupService(
    IConfiguration configuration,
    IWebHostEnvironment environment,
    IOptions<DataBackupOptions> backupOptions,
    ILogger<CustomerDataBackupService> logger) : BackgroundService
{
    private readonly DataBackupOptions options = backupOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.Enabled)
        {
            logger.LogInformation("Customer data backups are disabled.");
            return;
        }

        if (options.RunOnStartup)
        {
            await CreateBackupIfSupportedAsync(stoppingToken);
        }

        using var timer = new PeriodicTimer(TimeSpan.FromHours(options.IntervalHours));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CreateBackupIfSupportedAsync(stoppingToken);
        }
    }

    private async Task CreateBackupIfSupportedAsync(CancellationToken cancellationToken)
    {
        var provider = configuration["Database:Provider"]?.Trim().ToLowerInvariant() ?? "sqlite";
        if (provider is "postgres" or "postgresql")
        {
            logger.LogInformation("Skipping local customer data backup because the configured database provider is PostgreSQL.");
            return;
        }

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("Skipping customer data backup because the default connection string is not configured.");
            return;
        }

        var builder = new SqliteConnectionStringBuilder(connectionString);
        if (string.IsNullOrWhiteSpace(builder.DataSource))
        {
            logger.LogWarning("Skipping customer data backup because the SQLite data source is empty.");
            return;
        }

        var databasePath = builder.DataSource;
        if (!Path.IsPathRooted(databasePath))
        {
            databasePath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, databasePath));
        }

        if (!File.Exists(databasePath))
        {
            logger.LogWarning("Skipping customer data backup because the SQLite database file was not found at {DatabasePath}.", databasePath);
            return;
        }

        var backupDirectory = options.DirectoryPath;
        if (!Path.IsPathRooted(backupDirectory))
        {
            backupDirectory = Path.GetFullPath(Path.Combine(environment.ContentRootPath, backupDirectory));
        }

        Directory.CreateDirectory(backupDirectory);

        var backupFilePath = Path.Combine(
            backupDirectory,
            $"{Path.GetFileNameWithoutExtension(databasePath)}-{DateTime.UtcNow:yyyyMMddHHmmss}.db");

        await using var connection = new SqliteConnection($"Data Source={databasePath}");
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = $"VACUUM INTO '{backupFilePath.Replace("'", "''", StringComparison.Ordinal)}';";
        await command.ExecuteNonQueryAsync(cancellationToken);

        DeleteExpiredBackups(backupDirectory);
        logger.LogInformation("Created customer data backup at {BackupFilePath}.", backupFilePath);
    }

    private void DeleteExpiredBackups(string backupDirectory)
    {
        var retentionCutoff = DateTime.UtcNow.AddDays(-options.RetentionDays);
        foreach (var filePath in Directory.EnumerateFiles(backupDirectory, "*.db", SearchOption.TopDirectoryOnly))
        {
            if (File.GetCreationTimeUtc(filePath) >= retentionCutoff)
            {
                continue;
            }

            File.Delete(filePath);
        }
    }
}
