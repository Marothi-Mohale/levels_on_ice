using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LevelsOnIceSalon.Web.Tests.Infrastructure;

public sealed class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string seedDatabasePath = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "../../../../LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.dev.db"));
    private readonly string sqliteDatabasePath = Path.Combine(
        Path.GetTempPath(),
        $"levels-on-ice-tests-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(sqliteDatabasePath)!);
        File.Copy(seedDatabasePath, sqliteDatabasePath, overwrite: true);

        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ASPNETCORE_ENVIRONMENT"] = "Development",
                ["AdminAuth:Username"] = "test-admin",
                ["AdminAuth:Password"] = "test-password",
                ["AdminAuth:MfaSharedKey"] = "JBSWY3DPEHPK3PXP",
                ["Site:BaseUrl"] = "http://localhost",
                ["Captcha:Enabled"] = "false",
                ["App:UseHttpsRedirection"] = "false",
                ["DataBackups:Enabled"] = "false",
                ["DataBackups:RunOnStartup"] = "false",
                ["Database:Provider"] = "Sqlite",
                ["Database:ApplyMigrationsOnStartup"] = "false",
                ["Database:SeedSampleDataOnStartup"] = "false",
                ["ApiDocumentation:Enabled"] = "true",
                ["ApiTokens:SigningKey"] = "test-signing-key-with-at-least-32-bytes!!",
                ["ApiTokens:Issuer"] = "LevelsOnIceSalon.Tests",
                ["ApiTokens:Audience"] = "LevelsOnIceSalon.Tests.Api",
                ["ApiTokens:AccessTokenLifetimeMinutes"] = "60",
                ["ConnectionStrings:DefaultConnection"] = $"Data Source={sqliteDatabasePath}"
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        if (File.Exists(sqliteDatabasePath))
        {
            File.Delete(sqliteDatabasePath);
        }
    }
}
