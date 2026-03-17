using LevelsOnIceSalon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LevelsOnIceSalon.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string contentRootPath)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
        var provider = configuration["Database:Provider"]?.Trim().ToLowerInvariant() ?? "sqlite";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (provider == "postgresql" || provider == "postgres")
            {
                options.UseNpgsql(connectionString, npgsql =>
                    npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

                return;
            }

            var sqliteConnectionString = ResolveSqliteConnectionString(connectionString, contentRootPath);
            options.UseSqlite(sqliteConnectionString, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        return services;
    }

    private static string ResolveSqliteConnectionString(string connectionString, string contentRootPath)
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

            var fullPath = Path.GetFullPath(Path.Combine(contentRootPath, path));
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
