# Levels On Ice Salon

An ASP.NET Core 8 MVC portfolio project built to demonstrate the kind of full stack engineering expected in a modern product team: clean backend structure, practical database design, secure admin workflows, polished UI delivery, and production-conscious application behavior.

This repository is especially relevant for mid-level full stack roles centered on C#, .NET, SQL-backed applications, secure web delivery, API design, and shipping end-to-end features in a small, high-impact team.

## Why This Project Fits The Role

This codebase aligns well with a mid-level Full Stack Developer position focused on ASP.NET Core, SQL, secure engineering, and product delivery:

- Builds production-style features in C# and .NET 8
- Uses a layered architecture across web, infrastructure, and domain projects
- Works with EF Core and relational database patterns
- Includes authenticated admin functionality, JWT-secured API access, and security-minded configuration
- Delivers full stack user-facing features from persistence to UI
- Shows pragmatic engineering choices for a small team shipping quickly
- Exposes a versioned REST API with Swagger/OpenAPI
- Adds observability, structured logging, and production-aware diagnostics

It is not a one-to-one match for every item in the job description. This repo strongly demonstrates .NET, MVC, EF Core, SQL-backed workflows, secure configuration, versioned APIs, JWT auth foundations, Swagger/OpenAPI, and OpenTelemetry-ready observability. It does not currently showcase a TypeScript SPA, AWS infrastructure, Kubernetes, or a full OIDC/OAuth2 identity platform.

## Role-Relevant Skills Demonstrated

### Backend and API-Oriented Thinking

- ASP.NET Core 8 application structure with clear service registration and startup composition
- REST-style API controllers with versioned URL routing
- Swagger/OpenAPI documentation suitable for frontend client generation
- DTO-based API contracts with validation and ProblemDetails error responses
- Environment-aware configuration and fail-fast option validation
- Separation of concerns between web, infrastructure, and domain layers

The app is still MVC-first for the public website, but it now also exposes a cleaner public API surface for catalog-style data and API auth flows. That makes it useful both as a full stack web app and as a demonstration of production-oriented API design in ASP.NET Core.

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
- JWT bearer authentication for protected API endpoints
- Rate limiting and hardened cookie settings
- Secrets moved out of committed config and into environment variables or local user-secrets
- Production-minded static asset caching and response compression
- Consistent API error handling with ProblemDetails

### Operations and Diagnostics

- Versioned API routes under `/api/v1/...`
- Swagger/OpenAPI with XML comments and client-generation-friendly metadata
- Structured JSON logging with correlation and request identifiers
- Health checks for liveness and readiness
- OpenTelemetry-compatible metrics and distributed tracing
- Integration tests covering public API, auth, Swagger, health checks, and error behavior

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
- JWT bearer authentication
- Swagger / OpenAPI
- OpenTelemetry
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
4. API controllers return documented JSON contracts for frontend or integration consumers.
5. Razor views render the final server-side HTML for the public and admin site.

## Features

- Public marketing pages for home, about, services, gallery, testimonials, FAQs, booking, and contact
- Admin area for managing services, categories, FAQs, testimonials, gallery images, contact messages, appointment requests, opening hours, and site settings
- Database-backed booking and contact forms
- Search-engine-conscious metadata flow
- Image lazy loading and responsive image support where practical
- Static asset caching and response compression
- Upload validation and safer gallery image handling
- SQLite-first local setup with optional PostgreSQL configuration
- Versioned public REST API for service catalog data
- Swagger/OpenAPI docs with XML comments and versioned output
- JWT auth foundation for protected API routes
- Health endpoints, structured logging, and OpenTelemetry-ready observability

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

### API Overview

The backend now exposes a versioned API under `/api/v1/...`.

Public endpoints include:

- `GET /api/v1/service-categories`
- `GET /api/v1/service-categories/{slug}`
- `GET /api/v1/services`
- `GET /api/v1/services/{slug}`

Protected endpoints include:

- `POST /api/v1/auth/token`
- `GET /api/v1/auth/me`

The API uses:

- URL-path versioning
- DTO-based request and response contracts
- validation with ProblemDetails responses
- Swagger/OpenAPI metadata suitable for TypeScript or other generated clients

### Health, Logging, And Observability

Operational endpoints:

- `GET /health/live`
- `GET /health/ready`

Observability features:

- structured JSON console logging
- request and correlation identifiers
- OpenTelemetry tracing for inbound ASP.NET Core requests and outgoing `HttpClient` calls
- OpenTelemetry metrics for ASP.NET Core, Kestrel, `HttpClient`, and runtime telemetry
- OTLP exporter support through environment-driven configuration

### Required Configuration

Secrets are configured through environment variables or local `.NET user-secrets`:

- `AdminAuth__Username`
- `AdminAuth__Password`
- `AdminAuth__MfaSharedKey`
- `Captcha__SiteKey`
- `Captcha__SecretKey`
- `ConnectionStrings__DefaultConnection`
- `ApiTokens__Issuer`
- `ApiTokens__Audience`
- `ApiTokens__SigningKey`

Non-secret settings such as `Site__BaseUrl`, Swagger settings, and observability exporter settings can still be configured through appsettings or environment variables.

For local development, this project is already wired with a `UserSecretsId` in [LevelsOnIceSalon.Web.csproj](/workspaces/levels_on_ice/LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj), which means admin credentials and token signing keys can stay out of tracked files entirely.

```bash
dotnet user-secrets set "AdminAuth:Username" "your-admin-username" --project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj
dotnet user-secrets set "AdminAuth:Password" "your-admin-password" --project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj
dotnet user-secrets set "AdminAuth:MfaSharedKey" "YOUR_BASE32_TOTP_SECRET" --project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj
dotnet user-secrets set "ApiTokens:SigningKey" "a-long-random-signing-key" --project ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj
```

This is the recommended approach before merging to `main`, because `.NET user-secrets` are stored outside the repository and are not committed to Git.

### Example Local Environment Variables

```bash
export AdminAuth__Username="admin"
export AdminAuth__Password="use-a-long-random-password"
export AdminAuth__MfaSharedKey="YOUR_BASE32_TOTP_SECRET"
export ApiTokens__Issuer="LevelsOnIceSalon"
export ApiTokens__Audience="LevelsOnIceSalon.Clients"
export ApiTokens__SigningKey="a-long-random-signing-key"
export Site__BaseUrl="http://localhost:5099"

# Optional CAPTCHA
export Captcha__Enabled="true"
export Captcha__Provider="Turnstile"
export Captcha__SiteKey="your-site-key"
export Captcha__SecretKey="your-secret-key"

# Optional OTLP exporter
export Observability__Otlp__Endpoint="http://localhost:4317"
export Observability__Otlp__Protocol="grpc"
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

- a TypeScript frontend client
- cloud deployment on AWS
- CI/CD automation
- stronger automated test coverage
- deeper business-level metrics and dashboards
