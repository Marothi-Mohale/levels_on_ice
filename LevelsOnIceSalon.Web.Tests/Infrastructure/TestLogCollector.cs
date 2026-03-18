namespace LevelsOnIceSalon.Web.Tests.Infrastructure;

public sealed class TestLogCollector
{
    private readonly object syncRoot = new();
    private readonly List<TestLogEntry> entries = [];

    public void Add(TestLogEntry entry)
    {
        lock (syncRoot)
        {
            entries.Add(entry);
        }
    }

    public IReadOnlyList<TestLogEntry> Snapshot()
    {
        lock (syncRoot)
        {
            return entries.ToList();
        }
    }
}
