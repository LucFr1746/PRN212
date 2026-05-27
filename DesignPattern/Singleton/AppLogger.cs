namespace DesignPattern.Singleton;

public sealed class AppLogger
{
    private static readonly Lazy<AppLogger> _lazyInstance =
        new(() => new AppLogger());

    private readonly List<string> _logEntries = [];
    private int _logCount;

    private AppLogger()
    {
        _logCount = 0;
        Console.WriteLine("   [AppLogger] Instance created (this should appear ONLY ONCE)");
    }

    public static AppLogger Instance => _lazyInstance.Value;

    public void Log(LogLevel level, string message)
    {
        _logCount++;
        var entry = $"[{DateTime.Now:HH:mm:ss}] [{level}] #{_logCount}: {message}";
        _logEntries.Add(entry);
        Console.WriteLine($"   {entry}");
    }

    public void Info(string message) => Log(LogLevel.INFO, message);
    public void Warning(string message) => Log(LogLevel.WARNING, message);
    public void Error(string message) => Log(LogLevel.ERROR, message);

    public IReadOnlyList<string> GetAllLogs() => _logEntries.AsReadOnly();

    public int TotalLogCount => _logCount;
}

public enum LogLevel
{
    INFO,
    WARNING,
    ERROR
}
