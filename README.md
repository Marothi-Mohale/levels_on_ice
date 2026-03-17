# Levels On Ice Salon

A production-oriented ASP.NET Core MVC salon website built as a portfolio-quality project with a polished public brand experience, a lightweight admin area, and a pragmatic content workflow powered by EF Core.

This project is designed to showcase how a modern service business site can be delivered with strong fundamentals: clean separation between web, infrastructure, and domain layers; server-rendered performance; manageable content administration; and a deployment path that stays simple for smaller teams.

## Project Overview

Levels On Ice Salon is a multi-page beauty and salon website focused on:

- premium brand presentation
- service discovery and pricing
- appointment request capture
- contact lead capture
- gallery and testimonial storytelling
- admin-side content management

The application uses server-rendered Razor views instead of a heavy front-end SPA, which keeps the stack approachable, fast to load, and easier to host on modest infrastructure.

## Tech Stack

- ASP.NET Core 8 MVC
- Razor Views and partial-based UI composition
- C#
- Entity Framework Core 8
- SQLite for default local and lightweight production usage
- PostgreSQL-ready infrastructure for future scaling
- Cookie authentication for the admin area
- Native browser JavaScript for lightweight UI behavior
- CSS in `wwwroot` with asset versioning and production-friendly caching

## Features

- Public marketing pages for home, about, services, gallery, testimonials, FAQs, booking, and contact
- Premium salon-focused copy and content structure
- Admin area for managing services, categories, FAQs, testimonials, gallery images, contact messages, appointment requests, opening hours, and site settings
- Database-backed booking and contact forms
- Search-engine-conscious metadata flow
- Image lazy loading and responsive image support where practical
- Static asset caching and response compression
- Upload validation and safer gallery image handling
- SQLite-first local setup with optional PostgreSQL configuration

## Architecture Summary

The solution is split into three projects with clear responsibilities:

- `LevelsOnIceSalon.Web`
  Hosts the ASP.NET Core MVC application, controllers, services, Razor views, admin area, options binding, and web-specific orchestration.
- `LevelsOnIceSalon.Infrastructure`
  Owns EF Core, database context, entity configurations, migrations, provider setup, and seed support.
- `LevelsOnIceSalon.Domain`
  Contains the core entities, enums, and shared domain base types.

At a high level, the request flow looks like this:

1. A controller receives a request.
2. Web-layer services prepare page content, forms, and SEO metadata.
3. Infrastructure persists and queries data through `ApplicationDbContext`.
4. Razor views render the final server-side HTML.

This keeps the public site lean while making the project easy to extend without overengineering.

## Solution Structure

```text
.
|-- LevelsOnIceSalon.Domain/
|   |-- Common/
|   |-- Entities/
|   `-- Enums/
|-- LevelsOnIceSalon.Infrastructure/
|   |-- Data/
|   |   |-- Configurations/
|   |   |-- Converters/
|   |   |-- Migrations/
|   |   `-- Seed/
|   |-- DependencyInjection/
|   `-- Repositories/
|-- LevelsOnIceSalon.Web/
|   |-- Areas/
|   |   `-- Admin/
|   |       |-- Controllers/
|   |       |-- ViewModels/
|   |       `-- Views/
|   |-- Controllers/
|   |-- Data/
|   |-- Mapping/
|   |-- Options/
|   |-- Repositories/
|   |-- Services/
|   |-- ViewModels/
|   |-- Views/
|   |   `-- Shared/Partials/
|   `-- wwwroot/
|       |-- css/
|       |-- js/
|       |-- images/
|       `-- uploads/gallery/
`-- LevelsOnIceSalon.sln
```

## Local Setup

### Prerequisites

- .NET 8 SDK
- Git

### Run Locally

From the solution root:

```bash
dotnet tool restore
dotnet restore ./LevelsOnIceSalon.sln --configfile ./NuGet.Config
dotnet run --project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj
```

The app will start the MVC site locally. By default, development configuration points to SQLite and can apply migrations plus seed sample data on startup.

### Required Configuration

Before using the admin area outside throwaway local testing, set real values for:

- `AdminAuth:Username`
- `AdminAuth:Password`
- `Site:BaseUrl`

The app now validates key options on startup and will fail fast if required values are missing or left as placeholders.

## SQLite Setup

SQLite is the default database provider and the easiest way to run the project locally.

### Default SQLite Files

- Development database: `LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.dev.db`
- General database: `LevelsOnIceSalon.Web/App_Data/levels-on-ice-salon.db`

### Development Configuration

SQLite is already configured in:

- `LevelsOnIceSalon.Web/appsettings.Development.json`
- `LevelsOnIceSalon.Web/appsettings.json`

### Typical SQLite Workflow

1. Restore tools and packages.
2. Run the web project.
3. Let startup apply migrations and sample data in development, or run migrations manually if preferred.

If you want a clean local database, stop the app, remove the SQLite file in `App_Data`, and run migrations again.

## EF Core Migration Commands

Run all commands from the solution root.

### Restore the EF Core Tool

```bash
dotnet tool restore
```

### Add a Migration

```bash
dotnet ef migrations add YourMigrationName \
  --project ./LevelsOnIceSalon.Infrastructure/LevelsOnIceSalon.Infrastructure.csproj \
  --startup-project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj \
  --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

### Apply Migrations

```bash
dotnet ef database update \
  --project ./LevelsOnIceSalon.Infrastructure/LevelsOnIceSalon.Infrastructure.csproj \
  --startup-project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj \
  --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

### Remove the Last Migration

```bash
dotnet ef migrations remove \
  --project ./LevelsOnIceSalon.Infrastructure/LevelsOnIceSalon.Infrastructure.csproj \
  --startup-project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj \
  --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

## Key Implementation Notes

### Public Site

- Server-rendered pages keep the experience fast and SEO-friendly.
- Shared partials are used to compose branded sections across the site.
- Performance work includes response compression, static asset caching, image lazy loading, responsive images, and reduced JavaScript weight.

### Admin Area

- Cookie-authenticated admin area under `Areas/Admin`
- CRUD workflows for core content management
- Validation improvements for content editing and uploads
- Basic security hardening including safer cookie settings, upload checks, and login rate limiting

### Data Layer

- Entity configurations live in Infrastructure rather than in the entities themselves
- EF Core migrations are versioned in source control
- SQLite is the default provider, with PostgreSQL support still available for future migration

## Screenshots

Replace these placeholders with real product screenshots before publishing the project portfolio.

### Public Home Page

`docs/screenshots/home-page.png`

### Services Page

`docs/screenshots/services-page.png`

### Gallery Page

`docs/screenshots/gallery-page.png`

### Booking Flow

`docs/screenshots/booking-page.png`

### Admin Dashboard

`docs/screenshots/admin-dashboard.png`

## Deployment Notes

This project is suitable for small-to-medium production hosting with a lightweight infrastructure footprint.

### Production Checklist

- Set real `AdminAuth` credentials through environment-specific configuration or secret storage
- Set a correct public `Site:BaseUrl`
- Keep `App:UseHttpsRedirection` enabled in production
- Review `Database:ApplyMigrationsOnStartup` and `Database:SeedSampleDataOnStartup` before deployment
- Persist the `App_Data/DataProtection-Keys` directory across deployments if using multiple restarts or instances
- Confirm write access for `wwwroot/uploads/gallery` if admin uploads are enabled in production
- Serve the site behind HTTPS and, if applicable, a reverse proxy or CDN

### Static Assets

The app uses versioned static assets and long-lived cache headers. In production, that makes it safe to cache CSS, JS, and images aggressively.

### Minification And Bundling Guidance

The current front end is intentionally lightweight, so the recommended bundling strategy is a small `esbuild` step rather than a heavier SPA toolchain.

```bash
npx esbuild LevelsOnIceSalon.Web/wwwroot/js/site.js --bundle --minify --outfile=LevelsOnIceSalon.Web/wwwroot/js/site.min.js
npx esbuild LevelsOnIceSalon.Web/wwwroot/css/site.css --minify --outfile=LevelsOnIceSalon.Web/wwwroot/css/site.min.css
```

This approach fits the project well because:

- there is one primary CSS entry point
- there is one primary JavaScript entry point
- the site is server-rendered and does not need a complex client build system

## Future Improvements

- Replace shared config-based admin credentials with ASP.NET Identity or another proper multi-user admin system
- Add automated tests for services, controllers, validation, and critical page flows
- Refactor admin CRUD controllers toward a more consistent service-based pattern
- Add image processing or thumbnail generation as part of the upload pipeline
- Introduce CI validation for build, migrations, and formatting
- Add a real screenshot gallery and deployment diagram to strengthen the portfolio presentation
- Expand observability with structured logging and error monitoring integration

## Portfolio Positioning

This project is intentionally more than a brochure site. It demonstrates:

- full-stack .NET application structure
- pragmatic architecture for a real business website
- admin and content-management workflows
- production-minded performance and security improvements
- a polished, brand-led front-end experience without unnecessary framework weight
