using System.Collections.Concurrent;
using System.Globalization;
using System.Xml.Linq;

namespace LevelsOnIceSalon.Web.Services;

public class ImageMetadataService(IWebHostEnvironment environment) : IImageMetadataService
{
    private readonly string webRootPath = environment.WebRootPath;
    private readonly ConcurrentDictionary<string, ImageMetadata?> metadataCache = new(StringComparer.OrdinalIgnoreCase);

    public ImageMetadata? GetMetadata(string? imagePath)
    {
        var physicalPath = ResolvePhysicalPath(imagePath);
        if (physicalPath is null)
        {
            return null;
        }

        return metadataCache.GetOrAdd(physicalPath, ReadMetadata);
    }

    private string? ResolvePhysicalPath(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || !imagePath.StartsWith("/", StringComparison.Ordinal))
        {
            return null;
        }

        var relativePath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.GetFullPath(Path.Combine(webRootPath, relativePath));
        var normalizedRoot = Path.GetFullPath(webRootPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (!fullPath.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase) || !File.Exists(fullPath))
        {
            return null;
        }

        return fullPath;
    }

    private static ImageMetadata? ReadMetadata(string physicalPath)
    {
        return Path.GetExtension(physicalPath).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => ReadJpegMetadata(physicalPath),
            ".png" => ReadPngMetadata(physicalPath),
            ".svg" => ReadSvgMetadata(physicalPath),
            ".webp" => ReadWebpMetadata(physicalPath),
            _ => null
        };
    }

    private static ImageMetadata? ReadPngMetadata(string physicalPath)
    {
        using var stream = File.OpenRead(physicalPath);
        using var reader = new BinaryReader(stream);

        var signature = reader.ReadBytes(8);
        if (signature.Length < 8
            || signature[0] != 0x89
            || signature[1] != 0x50
            || signature[2] != 0x4E
            || signature[3] != 0x47
            || signature[4] != 0x0D
            || signature[5] != 0x0A
            || signature[6] != 0x1A
            || signature[7] != 0x0A)
        {
            return null;
        }

        _ = ReadBigEndianInt32(reader);
        if (new string(reader.ReadChars(4)) != "IHDR")
        {
            return null;
        }

        var width = ReadBigEndianInt32(reader);
        var height = ReadBigEndianInt32(reader);
        return width > 0 && height > 0 ? new ImageMetadata(width, height) : null;
    }

    private static ImageMetadata? ReadJpegMetadata(string physicalPath)
    {
        using var stream = File.OpenRead(physicalPath);

        if (stream.ReadByte() != 0xFF || stream.ReadByte() != 0xD8)
        {
            return null;
        }

        while (stream.Position < stream.Length)
        {
            var prefix = stream.ReadByte();
            if (prefix == -1)
            {
                break;
            }

            if (prefix != 0xFF)
            {
                continue;
            }

            var marker = stream.ReadByte();
            while (marker == 0xFF)
            {
                marker = stream.ReadByte();
            }

            if (marker is -1 or 0xD9 or 0xDA)
            {
                break;
            }

            if (marker is >= 0xD0 and <= 0xD7)
            {
                continue;
            }

            var segmentLength = ReadBigEndianUInt16(stream);
            if (segmentLength < 2)
            {
                return null;
            }

            if (marker is 0xC0 or 0xC1 or 0xC2 or 0xC3 or 0xC5 or 0xC6 or 0xC7 or 0xC9 or 0xCA or 0xCB or 0xCD or 0xCE or 0xCF)
            {
                _ = stream.ReadByte();
                var height = ReadBigEndianUInt16(stream);
                var width = ReadBigEndianUInt16(stream);
                return width > 0 && height > 0 ? new ImageMetadata(width, height) : null;
            }

            stream.Seek(segmentLength - 2, SeekOrigin.Current);
        }

        return null;
    }

    private static ImageMetadata? ReadSvgMetadata(string physicalPath)
    {
        var document = XDocument.Load(physicalPath);
        var root = document.Root;
        if (root is null)
        {
            return null;
        }

        if (TryParseSvgLength(root.Attribute("width")?.Value, out var width)
            && TryParseSvgLength(root.Attribute("height")?.Value, out var height))
        {
            return width > 0 && height > 0 ? new ImageMetadata(width, height) : null;
        }

        var viewBox = root.Attribute("viewBox")?.Value?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (viewBox is { Length: 4 }
            && double.TryParse(viewBox[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var viewBoxWidth)
            && double.TryParse(viewBox[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var viewBoxHeight))
        {
            return viewBoxWidth > 0 && viewBoxHeight > 0
                ? new ImageMetadata((int)Math.Round(viewBoxWidth), (int)Math.Round(viewBoxHeight))
                : null;
        }

        return null;
    }

    private static ImageMetadata? ReadWebpMetadata(string physicalPath)
    {
        using var stream = File.OpenRead(physicalPath);
        using var reader = new BinaryReader(stream);

        if (new string(reader.ReadChars(4)) != "RIFF")
        {
            return null;
        }

        _ = reader.ReadInt32();
        if (new string(reader.ReadChars(4)) != "WEBP")
        {
            return null;
        }

        var chunkType = new string(reader.ReadChars(4));
        return chunkType switch
        {
            "VP8X" => ReadWebpExtended(reader),
            "VP8 " => ReadWebpLossy(reader),
            "VP8L" => ReadWebpLossless(reader),
            _ => null
        };
    }

    private static ImageMetadata? ReadWebpExtended(BinaryReader reader)
    {
        _ = reader.ReadInt32();
        _ = reader.ReadByte();
        _ = reader.ReadBytes(3);
        var width = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
        var height = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
        return new ImageMetadata(width + 1, height + 1);
    }

    private static ImageMetadata? ReadWebpLossy(BinaryReader reader)
    {
        _ = reader.ReadInt32();
        _ = reader.ReadBytes(7);
        var startCode = reader.ReadBytes(3);
        if (startCode.Length != 3 || startCode[0] != 0x9D || startCode[1] != 0x01 || startCode[2] != 0x2A)
        {
            return null;
        }

        var width = reader.ReadUInt16() & 0x3FFF;
        var height = reader.ReadUInt16() & 0x3FFF;
        return width > 0 && height > 0 ? new ImageMetadata(width, height) : null;
    }

    private static ImageMetadata? ReadWebpLossless(BinaryReader reader)
    {
        _ = reader.ReadInt32();
        if (reader.ReadByte() != 0x2F)
        {
            return null;
        }

        var b0 = reader.ReadByte();
        var b1 = reader.ReadByte();
        var b2 = reader.ReadByte();
        var b3 = reader.ReadByte();

        var width = 1 + (((b1 & 0x3F) << 8) | b0);
        var height = 1 + (((b3 & 0x0F) << 10) | (b2 << 2) | ((b1 & 0xC0) >> 6));
        return width > 0 && height > 0 ? new ImageMetadata(width, height) : null;
    }

    private static bool TryParseSvgLength(string? value, out int pixels)
    {
        pixels = 0;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var numeric = new string(value
            .TakeWhile(character => char.IsDigit(character) || character is '.' or '-')
            .ToArray());

        if (!double.TryParse(numeric, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || result <= 0)
        {
            return false;
        }

        pixels = (int)Math.Round(result);
        return true;
    }

    private static int ReadBigEndianInt32(BinaryReader reader)
    {
        var bytes = reader.ReadBytes(4);
        if (bytes.Length < 4)
        {
            return 0;
        }

        return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
    }

    private static int ReadBigEndianUInt16(Stream stream)
    {
        var high = stream.ReadByte();
        var low = stream.ReadByte();
        return high == -1 || low == -1 ? 0 : (high << 8) | low;
    }
}
