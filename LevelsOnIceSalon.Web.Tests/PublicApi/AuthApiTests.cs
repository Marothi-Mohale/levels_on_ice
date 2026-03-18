using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using LevelsOnIceSalon.Web.Contracts.PublicApi;
using LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;
using LevelsOnIceSalon.Web.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Tests.PublicApi;

public sealed class AuthApiTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient client;

    public AuthApiTests(IntegrationTestWebApplicationFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateToken_WithValidCredentials_ReturnsBearerToken()
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/token", new CreateAccessTokenRequest
        {
            Username = "test-admin",
            Password = "test-password",
            OneTimeCode = GenerateTotp("JBSWY3DPEHPK3PXP")
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(JsonOptions);

        Assert.NotNull(token);
        Assert.False(string.IsNullOrWhiteSpace(token!.AccessToken));
        Assert.Equal("Bearer", token.TokenType);
        Assert.True(token.ExpiresIn > 0);
    }

    [Fact]
    public async Task CreateToken_WithInvalidCredentials_ReturnsProblemDetails()
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/token", new CreateAccessTokenRequest
        {
            Username = "test-admin",
            Password = "wrong-password",
            OneTimeCode = "000000"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);

        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status401Unauthorized, problem!.Status);
        Assert.Equal("Invalid credentials.", problem.Title);
    }

    [Fact]
    public async Task Me_WithoutToken_ReturnsProblemDetails()
    {
        var response = await client.GetAsync("/api/v1/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Me_WithBearerToken_ReturnsCurrentUser()
    {
        var tokenResponse = await client.PostAsJsonAsync("/api/v1/auth/token", new CreateAccessTokenRequest
        {
            Username = "test-admin",
            Password = "test-password",
            OneTimeCode = GenerateTotp("JBSWY3DPEHPK3PXP")
        });

        var token = await tokenResponse.Content.ReadFromJsonAsync<AccessTokenResponse>(JsonOptions);
        Assert.NotNull(token);

        using var authenticatedRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/auth/me");
        authenticatedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token!.AccessToken);

        var response = await client.SendAsync(authenticatedRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var currentUser = await response.Content.ReadFromJsonAsync<CurrentUserResponse>(JsonOptions);

        Assert.NotNull(currentUser);
        Assert.Equal("test-admin", currentUser!.Username);
        Assert.Contains("Admin", currentUser.Roles);
        Assert.Contains("mfa", currentUser.AuthenticationMethods);
    }

    private static string GenerateTotp(string base32Secret)
    {
        var secret = DecodeBase32(base32Secret);
        var counter = (long)Math.Floor(DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30d);
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
                throw new InvalidOperationException("The supplied test MFA secret is not valid Base32.");
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
