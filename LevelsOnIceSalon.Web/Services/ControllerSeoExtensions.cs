using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace LevelsOnIceSalon.Web.Services;

public static class ControllerSeoExtensions
{
    private const string SeoMetadataViewDataKey = "SeoMetadata";

    public static void ApplySeo(
        this Controller controller,
        string pageTitle,
        string description,
        string? socialImagePath = null,
        string openGraphType = "website")
    {
        var seoMetadataService = controller.HttpContext.RequestServices.GetRequiredService<ISeoMetadataService>();
        var metadata = seoMetadataService.BuildPageMetadata(pageTitle, description, socialImagePath, openGraphType);

        controller.ViewData["Title"] = pageTitle;
        controller.ViewData["MetaDescription"] = description;
        controller.ViewData[SeoMetadataViewDataKey] = metadata;
    }

    public static SeoMetadataViewModel? GetSeoMetadata(this ViewDataDictionary viewData)
    {
        return viewData[SeoMetadataViewDataKey] as SeoMetadataViewModel;
    }
}
