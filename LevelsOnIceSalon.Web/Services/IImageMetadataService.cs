namespace LevelsOnIceSalon.Web.Services;

public interface IImageMetadataService
{
    ImageMetadata? GetMetadata(string? imagePath);
}

public sealed record ImageMetadata(int Width, int Height);
