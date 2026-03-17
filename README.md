<<<<<<< HEAD
# Levels On Ice Salon

Initial production-oriented ASP.NET Core MVC solution scaffold for the Levels On Ice Salon marketing website.

## Solution Structure

- `LevelsOnIceSalon.Web`: MVC application, Razor views, admin area, view models, public UI
- `LevelsOnIceSalon.Infrastructure`: EF Core, PostgreSQL, repositories, dependency injection
- `LevelsOnIceSalon.Domain`: domain entities and shared abstractions

## Prerequisites

- .NET 8 SDK
- PostgreSQL 15+ or compatible managed PostgreSQL service

## Getting Started

1. Install the .NET 8 SDK.
2. Update the PostgreSQL connection string in:
   - `LevelsOnIceSalon.Web/appsettings.json`
   - `LevelsOnIceSalon.Web/appsettings.Development.json`
3. From the solution root, restore packages:

```powershell
dotnet restore
```

4. Create the database migration:

```powershell
dotnet ef migrations add InitialCreate --project .\LevelsOnIceSalon.Infrastructure\LevelsOnIceSalon.Infrastructure.csproj --startup-project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj
```

5. Apply the migration:

```powershell
dotnet ef database update --project .\LevelsOnIceSalon.Infrastructure\LevelsOnIceSalon.Infrastructure.csproj --startup-project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj
```

6. Run the web app:

```powershell
dotnet run --project .\LevelsOnIceSalon.Web\LevelsOnIceSalon.Web.csproj
```

## Initial Features Included

- ASP.NET Core MVC site shell
- Admin area shell
- EF Core DbContext configured for PostgreSQL
- Starter domain entities for services, gallery, FAQs, testimonials, bookings, and site settings
- Shared layout, partials, and starter theme assets
- Dependency injection registration entry points

## Notes

- The `wwwroot/images/salon/` folder is ready for salon imagery.
- The gallery import from Facebook should be handled as a controlled content workflow rather than live scraping.
=======
# levels_on_ice
Luxury Hair Salon Web Platform!
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
