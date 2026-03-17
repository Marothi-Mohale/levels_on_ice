using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LevelsOnIceSalon.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = ResolveBasePath();
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var (provider, connectionString) = ReadDatabaseSettings(basePath, environment);
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        if (provider == "postgresql" || provider == "postgres")
        {
            optionsBuilder.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        }
        else
        {
            optionsBuilder.UseSqlite(ResolveSqliteConnectionString(connectionString, basePath), sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        }

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    private static string ResolveBasePath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var webProjectDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "LevelsOnIceSalon.Web"));

        return Directory.Exists(webProjectDirectory)
            ? webProjectDirectory
            : currentDirectory;
    }

    private static (string Provider, string ConnectionString) ReadDatabaseSettings(string basePath, string environment)
    {
        var appSettings = ReadJsonFile(Path.Combine(basePath, "appsettings.json"));
        var environmentSettings = ReadJsonFile(Path.Combine(basePath, $"appsettings.{environment}.json"));

        var provider = ReadJsonString(environmentSettings, "Database", "Provider")
            ?? ReadJsonString(appSettings, "Database", "Provider")
            ?? "Sqlite";

        var connectionString = ReadJsonString(environmentSettings, "ConnectionStrings", "DefaultConnection")
            ?? ReadJsonString(appSettings, "ConnectionStrings", "DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        return (provider.Trim().ToLowerInvariant(), connectionString);
    }

    private static JsonDocument? ReadJsonFile(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        return JsonDocument.Parse(File.ReadAllText(path));
    }

    private static string? ReadJsonString(JsonDocument? document, string section, string key)
    {
        if (document?.RootElement.TryGetProperty(section, out var sectionElement) != true)
        {
            return null;
        }

        return sectionElement.TryGetProperty(key, out var valueElement)
            ? valueElement.GetString()
            : null;
    }

    private static string ResolveSqliteConnectionString(string connectionString, string basePath)
    {
        const string dataSourceToken = "Data Source=";

        if (!connectionString.StartsWith(dataSourceToken, StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        var segments = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        for (var index = 0; index < segments.Length; index++)
        {
            if (!segments[index].StartsWith(dataSourceToken, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var path = segments[index][dataSourceToken.Length..].Trim();
            if (string.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path))
            {
                return connectionString;
            }

            var fullPath = Path.GetFullPath(Path.Combine(basePath, path));
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            segments[index] = $"{dataSourceToken}{fullPath}";
            return string.Join(';', segments);
        }

        return connectionString;
    }
}
