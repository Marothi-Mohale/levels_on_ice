using System.Text.Json;
using LevelsOnIceSalon.Web.Options;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LevelsOnIceSalon.Web.Services;

public class SeoMetadataService(
    IHttpContextAccessor httpContextAccessor,
    IOptions<SiteOptions> siteOptions) : ISeoMetadataService
{
    private static readonly DateTime SitemapLastModifiedUtc = new(2026, 3, 17, 0, 0, 0, DateTimeKind.Utc);
    private readonly SiteOptions site = siteOptions.Value;

    public SeoMetadataViewModel BuildPageMetadata(
        string pageTitle,
        string description,
        string? socialImagePath = null,
        string openGraphType = "website")
    {
        var canonicalUrl = BuildAbsoluteUrl(GetRequestPath());
        var socialImageUrl = BuildAbsoluteUrl(string.IsNullOrWhiteSpace(socialImagePath)
            ? site.DefaultSocialImage
            : socialImagePath);
        var title = string.IsNullOrWhiteSpace(pageTitle) || pageTitle.Equals("Home", StringComparison.OrdinalIgnoreCase)
            ? $"{site.Name} | Premium Nails, Hair & Beauty Salon in Mowbray, Cape Town"
            : $"{pageTitle} | {site.Name}";

        var metadata = new SeoMetadataViewModel
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonicalUrl,
            OpenGraphType = openGraphType,
            OpenGraphTitle = title,
            OpenGraphDescription = description,
            OpenGraphUrl = canonicalUrl,
            OpenGraphImageUrl = socialImageUrl,
            OpenGraphImageAlt = $"Levels On Ice Salon {pageTitle} preview image",
            TwitterTitle = title,
            TwitterDescription = description,
            TwitterImageUrl = socialImageUrl,
            JsonLd = BuildStructuredDataJson(title, description, canonicalUrl)
        };

        return metadata;
    }

    public string BuildRobotsText()
    {
        var sitemapUrl = BuildAbsoluteUrl("/sitemap.xml");

        return string.Join(Environment.NewLine,
        [
            "User-agent: *",
            "Allow: /",
            "Disallow: /Admin",
            $"Sitemap: {sitemapUrl}"
        ]);
    }

    public IReadOnlyList<SitemapUrlEntry> BuildSitemapEntries()
    {
        return
        [
            CreateSitemapEntry("/", "weekly", 1.0m),
            CreateSitemapEntry("/about", "monthly", 0.8m),
            CreateSitemapEntry("/services", "weekly", 0.95m),
            CreateSitemapEntry("/gallery", "weekly", 0.9m),
            CreateSitemapEntry("/testimonials", "monthly", 0.8m),
            CreateSitemapEntry("/faqs", "monthly", 0.7m),
            CreateSitemapEntry("/contact", "monthly", 0.85m),
            CreateSitemapEntry("/book-appointment", "weekly", 0.95m)
        ];
    }

    private SitemapUrlEntry CreateSitemapEntry(string relativeUrl, string changeFrequency, decimal priority)
    {
        return new SitemapUrlEntry
        {
            Url = BuildAbsoluteUrl(relativeUrl),
            LastModifiedUtc = SitemapLastModifiedUtc,
            ChangeFrequency = changeFrequency,
            Priority = priority
        };
    }

    private string BuildStructuredDataJson(string title, string description, string canonicalUrl)
    {
        var structuredData = new object[]
        {
            new
            {
                @context = "https://schema.org",
                @type = "BeautySalon",
                name = site.Name,
                url = NormalizeBaseUrl(),
                image = BuildAbsoluteUrl(site.DefaultSocialImage),
                telephone = site.PhoneNumber,
                email = site.Email,
                priceRange = site.PriceRange,
                address = new
                {
                    @type = "PostalAddress",
                    streetAddress = site.StreetAddress,
                    addressLocality = site.AddressLocality,
                    addressRegion = site.AddressRegion,
                    postalCode = site.PostalCode,
                    addressCountry = site.AddressCountry
                },
                areaServed = new[]
                {
                    "Mowbray",
                    "Cape Town"
                },
                sameAs = BuildSameAsLinks()
            },
            new
            {
                @context = "https://schema.org",
                @type = "WebPage",
                name = title,
                description,
                url = canonicalUrl,
                about = site.Name,
                inLanguage = "en-ZA"
            }
        };

        return JsonSerializer.Serialize(structuredData);
    }

    private string[] BuildSameAsLinks()
    {
        return new[]
        {
            site.InstagramUrl,
            site.FacebookUrl,
            site.TikTokUrl
        }
        .Where(url => !string.IsNullOrWhiteSpace(url))
        .ToArray();
    }

    private string GetRequestPath()
    {
        var request = httpContextAccessor.HttpContext?.Request;
        return request?.Path.HasValue == true ? request.Path.Value! : "/";
    }

    private string BuildAbsoluteUrl(string pathOrUrl)
    {
        if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.ToString();
        }

        var baseUrl = NormalizeBaseUrl();
        if (string.IsNullOrWhiteSpace(pathOrUrl))
        {
            return baseUrl;
        }

        return $"{baseUrl.TrimEnd('/')}/{pathOrUrl.TrimStart('/')}";
    }

    private string NormalizeBaseUrl()
    {
        if (!string.IsNullOrWhiteSpace(site.BaseUrl))
        {
            return site.BaseUrl.TrimEnd('/');
        }

        var request = httpContextAccessor.HttpContext?.Request;
        if (request is not null)
        {
            return $"{request.Scheme}://{request.Host.Value}".TrimEnd('/');
        }

        return "http://localhost:5099";
    }
}
