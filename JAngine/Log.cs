using System.Diagnostics;
using System.Runtime.InteropServices;
using JAngine.Extensions;

namespace JAngine;

/// <summary>
/// Specifies how severe a log message is in various levels from debug to error.
/// </summary>
public enum Severity
{
    // Numbers are inflated to allow room in the future.
    /// <inheritdoc cref="Log.Debug(object?[])"/>
    Debug = 100,
    /// <inheritdoc cref="Log.Info(object?[])"/>
    Info = 200,
    /// <inheritdoc cref="Log.Warning(object?[])"/>
    Warning = 300,
    /// <inheritdoc cref="Log.Error(object?[])"/>
    Error = 400,
}

/// <summary>
/// Represents a single message to be logged.
/// </summary>
public class LogMessage
{
    private static readonly int s_severityFieldSize = Severity.Warning.ToString().Length;
    
    internal LogMessage(DateTime time, Severity severity, StackFrame? frame, params object?[] data)
    {
        Time = time;
        Severity = severity;
        Frame = frame;
        Data = data;
    }

    /// <summary>The time of the log message.</summary>
    public DateTime Time { get; init; }

    /// <summary>The severity of the message.</summary>
    public Severity Severity { get; init; }

    /// <summary>The stack frame the message originates from.</summary>
    public StackFrame? Frame { get; init; }

    /// <summary>The data to be logged.</summary>
    public object?[] Data { get; init; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        // TODO: Simulate some form of tab here instead of just splitting with " | ".
        string time = Time.ToString("yyyy/MM/dd HH:mm:ss.fff");
        string severity = Severity + new string(' ', s_severityFieldSize - Severity.ToString().Length);
        string origin = Frame is not null && Frame.GetMethod() is { } method
            ? (method.DeclaringType is not null ? method.DeclaringType.FullName + "." : "")
              + $"{method.Name}"
            : "";
        string message = string.Join(", ", Data);
        
        return $"{string.Join(" | ", time, severity, origin, message)}";
    }
}

internal sealed class TimerRecord
{
    // TODO: Consider collecting additional data to find outliers, mean etc. for each record.
    internal TimerRecord(string name)
    {
        Name = name;
        TotalTime = TimeSpan.Zero;
        Runs = 0;
    }

    public string Name { get; init; }
    public TimeSpan TotalTime { get; private set; }
    public int Runs { get; private set; }

    internal LogTimer Start(bool logAtEnd)
    {
        return new LogTimer(this, logAtEnd);
    }

    internal void Stop(Stopwatch stopwatch)
    {
        Runs += 1;
        TotalTime += stopwatch.Elapsed;
    }

    internal void Reset()
    {
        Runs = 0;
        TotalTime = TimeSpan.Zero;
    }
}

public sealed class LogTimer : IDisposable
{
    private readonly TimerRecord _record;
    private readonly bool _logAtEnd;
    private readonly Stopwatch _stopwatch;

    internal LogTimer(TimerRecord record, bool logAtEnd)
    {
        _record = record;
        _logAtEnd = logAtEnd;
        _stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        _record.Stop(_stopwatch);
        if (_logAtEnd)
        {
            Log.LogTimer(_record);
        }
    }
} 

/// <summary>
/// Provides the functionality of custom log messageGetFileLineNumber handlers.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Handle a single log message using the implemented method.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void HandleMessage(LogMessage message);
}

/// <summary>
/// Logs messages to the console.
/// Can be enabled or disabled using <see cref="ConsoleLogger.Enabled"/>
/// </summary>
public sealed class ConsoleLogger : ILogger
{
    void ILogger.HandleMessage(LogMessage message)
    {
        HandleMessage(message);
    }

    /// <summary>
    /// Enables or disables this logger.
    /// </summary>
    public static bool Enabled { get; set; } = true;
    /// <summary>
    /// The minimum severity of this logger.
    /// </summary>
    public static Severity MinimumSeverity { get; set; }

    private static ConsoleColor SeverityToColor(Severity severity) => severity switch
    {
        Severity.Debug => ConsoleColor.Gray,
        Severity.Info => ConsoleColor.White,
        Severity.Warning => ConsoleColor.Yellow,
        Severity.Error => ConsoleColor.Red,
        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
    };

    private static void HandleMessage(LogMessage message)
    {
        if (Enabled && message.Severity >= MinimumSeverity)
        {
            Console.ForegroundColor = SeverityToColor(message.Severity);
            Console.WriteLine(message);
        }
    }
}

/// <summary>
/// Logs messages to a file. Will log messages to Logs, and save a latest.txt for the latest log file.
/// Can be enabled or disabled using <see cref="FileLogger.Enabled"/>
/// </summary>
public sealed class FileLogger : ILogger
{
    // TODO: Through inheritance this class could allow writing messages to other files and filepaths. Or maybe through public static properties.
    [DllImport("Kernel32.dll", EntryPoint = "CreateHardLink", CharSet = CharSet.Unicode )]
    private static extern bool LinkWin(
        string lpFileName,
        string lpExistingFileName,
        IntPtr lpSecurityAttributes
    );

    [DllImport("libc", EntryPoint = "link")]
    private static extern int LinkLin(string oldpath, string newpath);
    
    void ILogger.HandleMessage(LogMessage message)
    {
        HandleMessage(message);
    }

    static FileLogger()
    {
        string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        if (Directory.Exists("Logs"))
        {
            string[] files = Directory.GetFiles("Logs", "*.txt");
            Array.Sort(files, string.CompareOrdinal);
            Array.Reverse(files);

            // Remove oldest if there is more than 10 logs.
            foreach (string file in files.Skip(10))
            {
                File.Delete(file);
            }
        }
        else
        {
            Directory.CreateDirectory("Logs");
        }

        string path = $"Logs/{time}.txt";
        FileStream fileStream = File.Open(path, FileMode.CreateNew, FileAccess.Write);
        s_streamWriter = new StreamWriter(fileStream);
        s_streamWriter.AutoFlush = true;

        // Create Latest.txt
        if (File.Exists("Logs/Latest.txt"))
        {
            File.Delete("Logs/Latest.txt");
        }
        
        if (OperatingSystem.IsWindows())
        {
            LinkWin("Logs/Latest.txt", path, IntPtr.Zero);
        }
        else if (OperatingSystem.IsLinux())
        {
            LinkLin( path, "Logs/Latest.txt");
        }
        else
        {
            Log.Warning("Couldn't create Logs/Latest.txt hardlink on this platform.");
        }
    }

    private static readonly StreamWriter s_streamWriter;
    /// <summary>
    /// Enables or disables this logger.
    /// </summary>
    public static bool Enabled { get; set; } = true;
    /// <summary>
    /// The minimum severity of this logger.
    /// </summary>
    public static Severity MinimumSeverity { get; set; }

    private static void HandleMessage(LogMessage message)
    {
        if (Enabled && message.Severity >= MinimumSeverity)
        {
            s_streamWriter.WriteLine(message.ToString());
        }
    }
}

/// <summary>
/// Logs messages of varying severity to the attached log handlers.
/// </summary>
public static class Log
{
    private static readonly Dictionary<string, TimerRecord> _timers = new Dictionary<string, TimerRecord>();
    private static readonly IReadOnlyList<ILogger> s_handlers;
    static Log()
    {
        s_handlers = Assemblies.CreateInstances<ILogger>().ToList();
        
        Log.Info("Logger initialized.");
    }

    private static void LogMessage(LogMessage message)
    {
        foreach (ILogger handler in s_handlers)
        {
            handler.HandleMessage(message);
        }
    }
    
    // TODO: Make sure this has as little overhead as possible.
    /// <summary>
    /// Starts a timer and optionally logs the time at the end.
    /// If the time is not logged at the end you can instead call <see cref="LogTimer(System.String)"/> with the same name.
    /// </summary>
    /// <param name="timerName">The name of the timer to log.</param>
    /// <param name="logAtEnd">If true the timer will be logged when stopped. Otherwise you will have to manually log it using <see cref="LogTimer(System.String)"/></param>
    /// <returns>The LogTimer keeping track of the current time. Call dispose on it to stop the timer again.</returns>
    public static LogTimer Time(string timerName, bool logAtEnd = true)
    {
        if (!_timers.TryGetValue(timerName, out TimerRecord? record))
        {
            record = new TimerRecord(timerName);
            _timers.Add(timerName, record);
        }

        return record.Start(logAtEnd);
    }

    internal static void LogTimer(TimerRecord record)
    {
        TimeSpan average = record.TotalTime / record.Runs;
        Info($"Timer {record.Name} has completed {record.Runs} runs with {average.ToStringFormatted()}/run and {record.TotalTime.ToStringFormatted()} total");
    }

    /// <summary>
    /// Logs the value of a timer by name.
    /// </summary>
    /// <param name="timerName">The name of the timer to log.</param>
    public static void LogTimer(string timerName)
    {
        if (!_timers.TryGetValue(timerName, out TimerRecord? record))
        {
            record = new TimerRecord(timerName);
            _timers.Add(timerName, record);
        }
        LogTimer(record);
    }

    /// <summary>
    /// Reset the value of a timer by name.
    /// </summary>
    /// <param name="timerName">The name of the timer to reset.</param>
    public static void ResetTimer(string timerName)
    {
        if (_timers.TryGetValue(timerName, out TimerRecord? record))
        {
            record.Reset();
        }
    }
    
    /// <summary>
    /// Sends a log message with a run-time specified message.
    /// </summary>
    /// <param name="severity">The severity of the log message.</param>
    /// <param name="data">The data of the message.</param>
    public static void Message(Severity severity, params object?[] data)
    {
        StackFrame? stack = new StackTrace().GetFrame(1);
        LogMessage message = new LogMessage(DateTime.Now, severity, stack, data);
        LogMessage(message);
    }
    
    /// <summary>
    /// Debug is used within the development stages of the game.
    /// </summary>
    /// <param name="data">The data of the message.</param>
    public static void Debug(params object?[] data)
    {
        StackFrame? stack = new StackTrace().GetFrame(1);
        LogMessage message = new LogMessage(DateTime.Now, Severity.Debug, stack, data);
        LogMessage(message);
    }

    /// <summary>
    /// Info is used to display information about the system, and the internal workings.
    /// </summary>
    /// <param name="data">The data of the message.</param>
    public static void Info(params object?[] data)
    {
        StackFrame? stack = new StackTrace().GetFrame(1);
        LogMessage message = new LogMessage(DateTime.Now, Severity.Info, stack, data);
        LogMessage(message);
    }

    /// <summary>
    /// Warnings are used to signal something went wrong, or something isn't working as expected, but program execution continues for now.
    /// </summary>
    /// <param name="data">The data of the message.</param>
    public static void Warning(params object?[] data)
    {
        StackFrame? stack = new StackTrace().GetFrame(1);
        LogMessage message = new LogMessage(DateTime.Now, Severity.Warning, stack, data);
        LogMessage(message);
    }

    /// <summary>
    /// Errors are used to signal something went wrong, and program execution can't continue beyond logging system state and closing resources.
    /// </summary>
    /// <param name="data">The data of the message.</param>
    public static void Error(params object?[] data)
    {
        StackFrame? stack = new StackTrace().GetFrame(1);
        LogMessage message = new LogMessage(DateTime.Now, Severity.Error, stack, data);
        LogMessage(message);
    }
    
    /// <inheritdoc cref="Log.Error(object?[])"/>
    /// <param name="exception">The exception to log.</param>
    public static void Error(Exception exception)
    {
        Error(exception, exception.StackTrace);
    }
}
