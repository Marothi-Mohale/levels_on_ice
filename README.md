# Levels On Ice Salon

An ASP.NET Core 8 MVC portfolio project built to demonstrate the kind of full stack engineering expected in a modern product team: clean backend structure, practical database design, secure admin workflows, polished UI delivery, and production-conscious application behavior.

This repository is especially relevant for mid-level full stack roles centered on C#, .NET, SQL-backed applications, secure web delivery, and shipping end-to-end features in a small, high-impact team.

## Why This Project Fits The Role

This codebase aligns well with a mid-level Full Stack Developer position focused on ASP.NET Core, SQL, secure engineering, and product delivery:

- Builds production-style features in C# and .NET 8
- Uses a layered architecture across web, infrastructure, and domain projects
- Works with EF Core and relational database patterns
- Includes authenticated admin functionality and security-minded configuration
- Delivers full stack user-facing features from persistence to UI
- Shows pragmatic engineering choices for a small team shipping quickly

It is not a one-to-one match for every item in the job description. This repo strongly demonstrates .NET, MVC, EF Core, SQL-backed workflows, secure configuration, and full stack feature delivery. It does not currently showcase a TypeScript SPA, AWS infrastructure, Kubernetes, OpenTelemetry, or OIDC/OAuth2 flows.

## Role-Relevant Skills Demonstrated

### Backend and API-Oriented Thinking

- ASP.NET Core 8 application structure with clear service registration and startup composition
- Controller and service-based request handling
- Environment-aware configuration and fail-fast option validation
- Separation of concerns between web, infrastructure, and domain layers

While this project is MVC-first rather than REST API-first, the same backend fundamentals apply directly to API development: dependency injection, validation, persistence orchestration, authentication, and production configuration.

### Full Stack Delivery

- End-to-end public site flows for browsing services, viewing gallery content, and submitting booking or contact forms
- Admin-side workflows for updating content and managing operational data
- Server-rendered Razor UI with lightweight JavaScript enhancements
- Responsive, content-driven pages designed for real-world business usage

### Data and Persistence

- EF Core 8 with migrations in source control
- SQLite as the default provider with PostgreSQL-ready infrastructure
- Structured entity configurations and database-backed content management
- Typical CRUD and query flows through `ApplicationDbContext`

### Security and Reliability

- Cookie-authenticated admin area
- MFA-protected admin sign-in
- Rate limiting and hardened cookie settings
- Secrets moved out of committed config and into environment variables
- Production-minded static asset caching and response compression

These choices align well with teams that care about secure engineering culture and OWASP-style thinking, even though this repository is not positioned as a formal security showcase.

## Project Overview

Levels On Ice Salon is a multi-page salon and beauty website focused on:

- premium brand presentation
- service discovery and pricing
- appointment request capture
- contact lead capture
- gallery and testimonial storytelling
- admin-side content management

The app uses server-rendered Razor views instead of a frontend SPA. That keeps the stack approachable, fast to load, and easier to operate for small teams while still demonstrating full stack product delivery.

## Tech Stack

- C#
- .NET 8
- ASP.NET Core MVC
- Razor Views
- Entity Framework Core 8
- SQLite
- PostgreSQL-ready provider support
- Cookie authentication
- Native browser JavaScript
- CSS and static assets in `wwwroot`

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

## Architecture Summary

The solution is split into three projects with clear responsibilities:

- `LevelsOnIceSalon.Web`
  Hosts the ASP.NET Core MVC app, controllers, Razor views, admin area, options binding, and web-specific orchestration.
- `LevelsOnIceSalon.Infrastructure`
  Owns EF Core, database context, migrations, provider setup, and seed support.
- `LevelsOnIceSalon.Domain`
  Contains the core entities, enums, and shared domain types.

At a high level, the request flow looks like this:

1. A controller receives a request.
2. Web-layer services prepare content, forms, and page metadata.
3. Infrastructure persists and queries data through `ApplicationDbContext`.
4. Razor views render the final server-side HTML.

## Features

- Public marketing pages for home, about, services, gallery, testimonials, FAQs, booking, and contact
- Admin area for managing services, categories, FAQs, testimonials, gallery images, contact messages, appointment requests, opening hours, and site settings
- Database-backed booking and contact forms
- Search-engine-conscious metadata flow
- Image lazy loading and responsive image support where practical
- Static asset caching and response compression
- Upload validation and safer gallery image handling
- SQLite-first local setup with optional PostgreSQL configuration

## Screenshots

### Home Page

![Home page screenshot](docs/screenshots/home-page.png)

### Services Page

![Services page screenshot](docs/screenshots/services-page.png)

### Gallery Page

![Gallery page screenshot](docs/screenshots/gallery-page.png)

### Booking Page

![Booking page screenshot](docs/screenshots/booking-page.png)

### Admin Dashboard

![Admin dashboard screenshot](docs/screenshots/admin-dashboard.png)

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

By default, development configuration points to SQLite and can apply migrations plus seed sample data on startup.

### Swagger And OpenAPI

The public API exposes Swagger UI at `/swagger` when `ApiDocumentation:Enabled=true`. Development config enables this by default, while production config keeps it disabled unless you opt in explicitly.

For production deployments, the safer default is:

- keep `ApiDocumentation:Enabled=false` on public environments
- enable it only for internal review, staging, or protected environments
- treat the generated `/swagger/v1/swagger.json` file as the contract used for client generation

### Required Configuration

Secrets are configured through environment variables:

- `AdminAuth__Username`
- `AdminAuth__Password`
- `AdminAuth__MfaSharedKey`
- `Captcha__SiteKey`
- `Captcha__SecretKey`
- `ConnectionStrings__DefaultConnection`

Non-secret settings such as `Site__BaseUrl` can still be configured through appsettings or environment variables.

### Example Local Environment Variables

```bash
export AdminAuth__Username="admin"
export AdminAuth__Password="use-a-long-random-password"
export AdminAuth__MfaSharedKey="YOUR_BASE32_TOTP_SECRET"
export Site__BaseUrl="http://localhost:5099"

# Optional CAPTCHA
export Captcha__Enabled="true"
export Captcha__Provider="Turnstile"
export Captcha__SiteKey="your-site-key"
export Captcha__SecretKey="your-secret-key"
```

## EF Core Commands

Run all commands from the solution root.

### Restore The EF Core Tool

```bash
dotnet tool restore
```

### Add A Migration

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

### Remove The Last Migration

```bash
dotnet ef migrations remove \
  --project ./LevelsOnIceSalon.Infrastructure/LevelsOnIceSalon.Infrastructure.csproj \
  --startup-project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj \
  --context LevelsOnIceSalon.Infrastructure.Data.ApplicationDbContext
```

## Notes For Reviewers And Hiring Teams

If you are reviewing this project against a role similar to:

- ASP.NET Core and C# backend development
- EF Core and relational database work
- secure web application delivery
- full stack ownership in a small engineering team

then the strongest signals in this repo are the application structure, database-backed content workflows, security-conscious admin implementation, and the ability to ship complete features across backend and UI layers.

If needed, this project could be extended next with:

- REST APIs with Swagger/OpenAPI
- a TypeScript frontend client
- cloud deployment on AWS
- CI/CD automation
- observability instrumentation
- stronger automated test coverage
