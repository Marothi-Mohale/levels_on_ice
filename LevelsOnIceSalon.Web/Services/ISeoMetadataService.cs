using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface ISeoMetadataService
{
    SeoMetadataViewModel BuildPageMetadata(
        string pageTitle,
        string description,
        string? socialImagePath = null,
        string openGraphType = "website");

    string BuildRobotsText();

    IReadOnlyList<SitemapUrlEntry> BuildSitemapEntries();
}
