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
dotnet restore .\LevelsOnIceSalon.sln --configfile .\NuGet.Config
```

2. Run the web app:

```powershell
dotnet run --project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj
```

3. On first startup, the app will:
   - create the SQLite database automatically
   - create the schema
   - seed sample services, FAQs, contact details, and opening hours

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
