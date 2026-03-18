using Microsoft.Extensions.Logging;

namespace LevelsOnIceSalon.Web.Tests.Infrastructure;

public sealed class TestLogEntry
{
    public string Category { get; init; } = string.Empty;

    public LogLevel LogLevel { get; init; }

    public string Message { get; init; } = string.Empty;

    public Exception? Exception { get; init; }

    public IReadOnlyDictionary<string, object?> Properties { get; init; } = new Dictionary<string, object?>();
}
