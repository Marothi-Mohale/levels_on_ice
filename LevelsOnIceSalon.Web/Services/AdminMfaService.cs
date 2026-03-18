using System.Security.Cryptography;
using LevelsOnIceSalon.Web.Options;
using Microsoft.Extensions.Options;

namespace LevelsOnIceSalon.Web.Services;

public class AdminMfaService(IOptions<AdminAuthOptions> adminAuthOptions) : IAdminMfaService
{
    private static readonly TimeSpan TimeStep = TimeSpan.FromSeconds(30);
    private readonly AdminAuthOptions options = adminAuthOptions.Value;

    public bool IsRequired => options.RequireMfa;

    public bool ValidateCode(string? code)
    {
        if (!options.RequireMfa)
        {
            return true;
        }

        var normalizedCode = new string((code ?? string.Empty).Where(char.IsDigit).ToArray());
        if (normalizedCode.Length != 6)
        {
            return false;
        }

        var secret = DecodeBase32(options.MfaSharedKey);
        var currentCounter = GetCurrentCounter();

        for (var offset = -1; offset <= 1; offset++)
        {
            if (string.Equals(normalizedCode, ComputeCode(secret, currentCounter + offset), StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static long GetCurrentCounter() =>
        (long)Math.Floor(DateTimeOffset.UtcNow.ToUnixTimeSeconds() / TimeStep.TotalSeconds);

    private static string ComputeCode(byte[] secret, long counter)
    {
        Span<byte> counterBytes = stackalloc byte[8];
        for (var index = 7; index >= 0; index--)
        {
            counterBytes[index] = (byte)(counter & 0xff);
            counter >>= 8;
        }

        using var hmac = new HMACSHA1(secret);
        var hash = hmac.ComputeHash(counterBytes.ToArray());
        var offset = hash[^1] & 0x0f;
        var binaryCode =
            ((hash[offset] & 0x7f) << 24) |
            (hash[offset + 1] << 16) |
            (hash[offset + 2] << 8) |
            hash[offset + 3];

        return (binaryCode % 1_000_000).ToString("D6");
    }

    private static byte[] DecodeBase32(string value)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        var normalized = new string(value
            .Trim()
            .ToUpperInvariant()
            .Where(character => !char.IsWhiteSpace(character) && character != '=')
            .ToArray());

        var output = new List<byte>();
        var bitBuffer = 0;
        var bitsInBuffer = 0;

        foreach (var character in normalized)
        {
            var charIndex = alphabet.IndexOf(character);
            if (charIndex < 0)
            {
                throw new InvalidOperationException("Admin MFA secret must be valid Base32.");
            }

            bitBuffer = (bitBuffer << 5) | charIndex;
            bitsInBuffer += 5;

            if (bitsInBuffer < 8)
            {
                continue;
            }

            bitsInBuffer -= 8;
            output.Add((byte)(bitBuffer >> bitsInBuffer));
            bitBuffer &= (1 << bitsInBuffer) - 1;
        }

        return output.ToArray();
    }
}
