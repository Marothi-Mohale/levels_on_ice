using System.Xml.Linq;
using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

public class SeoController(ISeoMetadataService seoMetadataService) : Controller
{
    [HttpGet("robots.txt")]
    public IActionResult Robots()
    {
        return Content(seoMetadataService.BuildRobotsText(), "text/plain");
    }

    [HttpGet("sitemap.xml")]
    public IActionResult Sitemap()
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        var document = new XDocument(
            new XElement(ns + "urlset",
                seoMetadataService.BuildSitemapEntries().Select(entry =>
                    new XElement(ns + "url",
                        new XElement(ns + "loc", entry.Url),
                        new XElement(ns + "lastmod", entry.LastModifiedUtc.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", entry.ChangeFrequency),
                        new XElement(ns + "priority", entry.Priority.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture))))));

        return Content(document.ToString(), "application/xml");
    }
}
