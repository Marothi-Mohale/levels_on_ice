using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Data;
using LevelsOnIceSalon.Web.Extensions;
using LevelsOnIceSalon.Infrastructure.DependencyInjection;
using LevelsOnIceSalon.Web.Middleware;
using LevelsOnIceSalon.Web.OpenApi;
using LevelsOnIceSalon.Web.Options;
using LevelsOnIceSalon.Web.Observability;
using LevelsOnIceSalon.Web.Security;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text;
using System.IO.Compression;
using System.Security.Claims;
using System.Threading.RateLimiting;

static bool ShouldTraceRequest(HttpContext context)
{
    var path = context.Request.Path;
    if (path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    var extension = Path.GetExtension(path.Value);
    return !string.Equals(extension, ".css", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".js", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".svg", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".webp", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".woff", StringComparison.OrdinalIgnoreCase)
        && !string.Equals(extension, ".woff2", StringComparison.OrdinalIgnoreCase);
}

var builder = WebApplication.CreateBuilder(args);
var dataProtectionKeysPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtection-Keys");

Directory.CreateDirectory(dataProtectionKeysPath);

builder.Logging.ClearProviders();
builder.Logging.Configure(options =>
{
    options.ActivityTrackingOptions =
        ActivityTrackingOptions.TraceId |
        ActivityTrackingOptions.SpanId |
        ActivityTrackingOptions.ParentId;
});
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "O";
    options.UseUtcTimestamp = true;
    options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions
    {
        Indented = false
    };
});
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddDebug();
}

builder.Services
    .AddOptions<AdminAuthOptions>()
    .Bind(builder.Configuration.GetSection(AdminAuthOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password),
        "AdminAuth credentials must be configured through environment variables.")
    .Validate(
        options => !options.RequireMfa || !string.IsNullOrWhiteSpace(options.MfaSharedKey),
        "Admin MFA must be configured through AdminAuth__MfaSharedKey when MFA is required.")
    .ValidateOnStart();
builder.Services
    .AddOptions<CaptchaOptions>()
    .Bind(builder.Configuration.GetSection(CaptchaOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(
        options => !options.Enabled
            || (!string.IsNullOrWhiteSpace(options.SiteKey) && !string.IsNullOrWhiteSpace(options.SecretKey)),
        "CAPTCHA is enabled but Captcha__SiteKey and Captcha__SecretKey are not configured.")
    .ValidateOnStart();
builder.Services
    .AddOptions<DataBackupOptions>()
    .Bind(builder.Configuration.GetSection(DataBackupOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services
    .AddOptions<SiteOptions>()
    .Bind(builder.Configuration.GetSection(SiteOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(
        options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _),
        "Site:BaseUrl must be a valid absolute URL.")
    .ValidateOnStart();
builder.Services
    .AddOptions<ApiTokenOptions>()
    .Bind(builder.Configuration.GetSection(ApiTokenOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services
    .AddOptions<ObservabilityOptions>()
    .Bind(builder.Configuration.GetSection(ObservabilityOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddProblemDetails();
builder.Services.AddControllersWithViews();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddMvc().AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Detail = "The request contains invalid query string or body values.",
            Instance = context.HttpContext.Request.Path
        };

        problemDetails.AddRequestCorrelation(context.HttpContext);

        return new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = { "application/problem+json" }
        };
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<Microsoft.Extensions.Options.IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<SelfHealthCheck>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.Configure<HttpsRedirectionOptions>(options =>
{
    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
    options.HttpsPort = null;
});
builder.Services.AddResponseCaching();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("admin-login", context =>
    {
        var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(5),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });
    options.AddPolicy("api-token", context =>
    {
        var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["image/svg+xml"]);
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.ContentRootPath);
builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddCheck<SelfHealthCheck>(
        "self",
        failureStatus: HealthStatus.Unhealthy,
        tags: [ObservabilityConstants.LiveHealthTag])
    .AddDbContextCheck<ApplicationDbContext>(
        "database",
        failureStatus: HealthStatus.Unhealthy,
        tags: [ObservabilityConstants.ReadyHealthTag]);

var observabilityOptions = builder.Configuration
    .GetSection(ObservabilityOptions.SectionName)
    .Get<ObservabilityOptions>() ?? new ObservabilityOptions();
var serviceVersion = string.IsNullOrWhiteSpace(observabilityOptions.ServiceVersion)
    ? typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0"
    : observabilityOptions.ServiceVersion;

var openTelemetryBuilder = builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: observabilityOptions.ServiceName,
            serviceVersion: serviceVersion,
            serviceInstanceId: Environment.MachineName)
        .AddAttributes([
            new KeyValuePair<string, object>("deployment.environment", builder.Environment.EnvironmentName)
        ]));

openTelemetryBuilder.WithTracing(tracing =>
{
    tracing
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
            options.Filter = ShouldTraceRequest;
        })
        .AddHttpClientInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddSource(ObservabilityConstants.ActivitySourceName)
        .AddSource("Microsoft.EntityFrameworkCore");

    if (Uri.TryCreate(observabilityOptions.Otlp.Endpoint, UriKind.Absolute, out var tracingEndpoint))
    {
        tracing.AddOtlpExporter(options =>
        {
            options.Endpoint = tracingEndpoint;
            options.Protocol = string.Equals(observabilityOptions.Otlp.Protocol, "http/protobuf", StringComparison.OrdinalIgnoreCase)
                ? OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf
                : OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        });
    }
    else if (observabilityOptions.Console.TracingEnabled)
    {
        tracing.AddConsoleExporter();
    }
});

openTelemetryBuilder.WithMetrics(metrics =>
{
    metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddMeter(ObservabilityConstants.MeterName)
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        .AddMeter("System.Net.Http");

    if (Uri.TryCreate(observabilityOptions.Otlp.Endpoint, UriKind.Absolute, out var metricsEndpoint))
    {
        metrics.AddOtlpExporter(options =>
        {
            options.Endpoint = metricsEndpoint;
            options.Protocol = string.Equals(observabilityOptions.Otlp.Protocol, "http/protobuf", StringComparison.OrdinalIgnoreCase)
                ? OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf
                : OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        });
    }
    else if (observabilityOptions.Console.MetricsEnabled)
    {
        metrics.AddConsoleExporter();
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.AccessDeniedPath = "/admin/login";
        options.Cookie.Name = "LevelsOnIceSalon.AdminAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    })
    .AddJwtBearer(AuthConstants.BearerScheme, _ => { });
builder.Services.AddOptions<JwtBearerOptions>(AuthConstants.BearerScheme)
    .Configure<IOptions<ApiTokenOptions>, IWebHostEnvironment>((options, apiTokenOptions, environment) =>
    {
        var tokenOptions = apiTokenOptions.Value;
        var signingKeyBytes = Encoding.UTF8.GetBytes(tokenOptions.SigningKey);

        options.RequireHttpsMetadata = !environment.IsDevelopment();
        options.IncludeErrorDetails = environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
            ValidateIssuer = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenOptions.Audience,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                if (context.Response.HasStarted)
                {
                    return;
                }

                context.HandleResponse();
                await AuthenticationProblemDetailsWriter.WriteAsync(
                    context.HttpContext,
                    StatusCodes.Status401Unauthorized,
                    "Authentication required.",
                    "A valid bearer access token is required to access this resource.",
                    context.HttpContext.RequestAborted);
            },
            OnForbidden = context =>
                AuthenticationProblemDetailsWriter.WriteAsync(
                    context.HttpContext,
                    StatusCodes.Status403Forbidden,
                    "Forbidden.",
                    "The authenticated principal does not have access to this resource.",
                    context.HttpContext.RequestAborted)
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthConstants.ApiAdminPolicy, policy =>
    {
        policy.AddAuthenticationSchemes(AuthConstants.BearerScheme);
        policy.RequireAuthenticatedUser();
        policy.RequireRole(AuthConstants.AdminRole);
    });
});
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath))
    .SetApplicationName("LevelsOnIceSalon");

var app = builder.Build();
var swaggerEnabled = app.Configuration.GetValue<bool?>("ApiDocumentation:Enabled") ?? app.Environment.IsDevelopment();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseForwardedHeaders();
app.UseMiddleware<RequestCorrelationMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ApiExceptionHandlingMiddleware>();
app.UseMiddleware<SuspiciousTrafficLoggingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

if (!app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("App:UseHttpsRedirection"))
{
    app.UseHttpsRedirection();
}

app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        var typedHeaders = context.Context.Response.GetTypedHeaders();
        typedHeaders.CacheControl = new CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromDays(365)
        };
        context.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=31536000,immutable";
        typedHeaders.Expires = DateTimeOffset.UtcNow.AddDays(365);
        context.Context.Response.Headers[HeaderNames.XContentTypeOptions] = "nosniff";
    }
});
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseRouting();
app.UseRateLimiter();
app.UseResponseCaching();

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Levels On Ice Salon Public API {description.GroupName.ToUpperInvariant()}");
        }

        options.DocumentTitle = "Levels On Ice Salon Public API Docs";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains(ObservabilityConstants.LiveHealthTag),
    ResponseWriter = (context, report) => HealthCheckResponseWriter.WriteJsonAsync(context, report)
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains(ObservabilityConstants.ReadyHealthTag),
    ResponseWriter = (context, report) => HealthCheckResponseWriter.WriteJsonAsync(context, report)
});
app.MapControllers();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await SeedData.InitializeAsync(
    app.Services,
    applyMigrations: app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"),
    seedSampleData: app.Configuration.GetValue<bool>("Database:SeedSampleDataOnStartup"));

app.Run();

public partial class Program;
