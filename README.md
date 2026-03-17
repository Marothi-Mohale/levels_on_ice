# Levels On Ice Salon

Production-oriented ASP.NET Core MVC solution for the Levels On Ice Salon marketing website.

## Solution Structure

- `LevelsOnIceSalon.Web`: MVC application, Razor views, public site, admin area shell, view models
- `LevelsOnIceSalon.Infrastructure`: EF Core, database configuration, dependency injection, seed helpers
- `LevelsOnIceSalon.Domain`: domain entities, enums, and shared abstractions

## Current Database Strategy

- Default provider: `SQLite`
- Default development database: `LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.dev.db`
- Default general database: `LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.db`
- PostgreSQL support is still available in configuration for later migration when scaling requires it

## Prerequisites

- .NET 8 SDK

## Getting Started

1. Restore packages:

```powershell
dotnet tool restore
dotnet restore .\LevelsOnIceSalon.sln --configfile .\NuGet.Config
```

2. Run the web app:

```powershell
dotnet run --project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj
```

3. On first startup, the app will:
   - apply EF Core migrations automatically
   - seed sample services, FAQs, contact details, and opening hours

## SQLite Configuration

SQLite is fully configured as the default provider for both development and lightweight production use.

- Provider package: `Microsoft.EntityFrameworkCore.Sqlite`
- Development connection string: `LevelsOnIceSalon.Web/appsettings.Development.json`
- General connection string: `LevelsOnIceSalon.Web/appsettings.json`
- Design-time EF Core support: `LevelsOnIceSalon.Infrastructure/Data/ApplicationDbContextFactory.cs`

The SQLite database files are stored under:

- `LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.dev.db`
- `LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.db`

## EF Core Migrations

Run these commands from the solution root:

1. Restore the local EF tool:

```powershell
dotnet tool restore
```

2. Create a new migration:

```powershell
dotnet ef migrations add YourMigrationName --project .\LevelsOnIceSalon.Infrastructure\LevelsOnIceSalon.Infrastructure.csproj --startup-project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

3. Apply migrations to the configured SQLite database:

```powershell
dotnet ef database update --project .\LevelsOnIceSalon.Infrastructure\LevelsOnIceSalon.Infrastructure.csproj --startup-project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

4. If you want to remove the last migration before applying it:

```powershell
dotnet ef migrations remove --project .\LevelsOnIceSalon.Infrastructure\LevelsOnIceSalon.Infrastructure.csproj --startup-project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

## Date, Time, and Money Handling

- `DateOnly` and `TimeOnly` are used for booking dates, preferred times, opening hours, and promotion schedules.
- Audit timestamps are normalized to UTC through EF Core value converters.
- Service prices use `decimal` with precision configured as `(10,2)` through Fluent API.
- Seed data only inserts application data when the target tables are empty, so startup remains safe and repeatable.

## Configuration

SQLite is the default option in:

- `LevelsOnIceSalon.Web/appsettings.json`
- `LevelsOnIceSalon.Web/appsettings.Development.json`

If you later want PostgreSQL again:

1. Set `"Database": { "Provider": "PostgreSql" }`
2. Update `ConnectionStrings:DefaultConnection` to a PostgreSQL connection string
3. Use EF Core migrations against PostgreSQL as needed

## Initial Features Included

- ASP.NET Core MVC site shell
- premium public marketing pages
- contact and booking forms saved to the database
- gallery with local image fallback
- SQLite-first startup for easier local and lightweight production hosting
- PostgreSQL-ready infrastructure for future migration

## Notes

- The `wwwroot/images/salon/` folder is ready for your real salon imagery.
- Facebook gallery importing should remain a controlled content workflow rather than live scraping.

## Production Performance Notes

- Static assets in `wwwroot` are fingerprinted in the views with `asp-append-version="true"` and are now safe to cache aggressively at the HTTP layer.
- Public image-heavy views reserve space for images early, lazy-load non-critical media, and prefer thumbnail-sized gallery assets when available.
- The public site no longer depends on the Bootstrap JavaScript bundle for navigation, FAQs, and the gallery lightbox, which reduces shipped JavaScript and avoids loading a heavier runtime than this site needs.

## Minification And Bundling Strategy

This repo currently keeps CSS and JS intentionally lightweight, so the recommended production path is:

1. Keep authoring in:
   - `LevelsOnIceSalon.Web/wwwroot/css/site.css`
   - `LevelsOnIceSalon.Web/wwwroot/js/site.js`
2. Add a very small build step with `esbuild` instead of a larger front-end toolchain:

```bash
npx esbuild LevelsOnIceSalon.Web/wwwroot/js/site.js --bundle --minify --outfile=LevelsOnIceSalon.Web/wwwroot/js/site.min.js
npx esbuild LevelsOnIceSalon.Web/wwwroot/css/site.css --minify --outfile=LevelsOnIceSalon.Web/wwwroot/css/site.min.css
```

3. In production, point the layouts at the minified files while keeping `asp-append-version="true"` for cache busting.

Why this approach:

- `esbuild` is fast and small.
- It avoids introducing a heavier SPA-style pipeline for a server-rendered MVC site.
- It fits the current codebase, where there is one main CSS file and one main JS file.
