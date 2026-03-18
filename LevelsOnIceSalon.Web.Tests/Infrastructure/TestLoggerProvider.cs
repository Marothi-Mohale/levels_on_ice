using System.Collections;
using Microsoft.Extensions.Logging;

namespace LevelsOnIceSalon.Web.Tests.Infrastructure;

public sealed class TestLoggerProvider(TestLogCollector collector) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new TestLogger(categoryName, collector);

    public void Dispose()
    {
    }

    private sealed class TestLogger(string categoryName, TestLogCollector collector) : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            collector.Add(new TestLogEntry
            {
                Category = categoryName,
                LogLevel = logLevel,
                Message = formatter(state, exception),
                Exception = exception,
                Properties = ToProperties(state)
            });
        }

        private static IReadOnlyDictionary<string, object?> ToProperties<TState>(TState state)
        {
            if (state is IEnumerable<KeyValuePair<string, object?>> structuredState)
            {
                return structuredState.ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            if (state is IDictionary dictionary)
            {
                var properties = new Dictionary<string, object?>();
                foreach (DictionaryEntry entry in dictionary)
                {
                    properties[entry.Key?.ToString() ?? string.Empty] = entry.Value;
                }

                return properties;
            }

            return new Dictionary<string, object?>();
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
