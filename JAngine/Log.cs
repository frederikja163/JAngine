using System.Runtime.CompilerServices;

namespace JAngine;

/// <summary>
/// Severity of a log message.
/// </summary>
public enum Severity
{
    Trace,
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}

/// <summary>
/// Interface for handlers of log messages.
/// </summary>
public interface ILogHandler
{
    /// <summary>
    /// Method for handling a single log message.
    /// </summary>
    /// <param name="message">The message to be handled.</param>
    public void HandleMessage(Message message);
}

/// <summary>
/// Log messages to the console.
/// </summary>
public sealed class ConsoleLogHandler : ILogHandler
{
    /// <inheritdoc cref="ILogHandler.HandleMessage"/>
    public void HandleMessage(Message message)
    {
        Console.WriteLine(message.ToString());
    }
}

public sealed class FileLogHandler : IDisposable, ILogHandler
{
    private readonly FileStream _fileStream;
    private readonly StreamWriter _streamWriter;
    
    /// <summary>
    /// Creates a new <see cref="FileLogHandler"/> with a filepath to log messages to.
    /// </summary>
    /// <param name="path">The path to log messages to.</param>
    public FileLogHandler(string path)
    {
        string? dirName = Path.GetDirectoryName(path);
        if (!Directory.Exists(dirName) && dirName != null)
        {
            Directory.CreateDirectory(dirName);
        }
        
        _fileStream = File.OpenWrite(path);
        _streamWriter = new StreamWriter(_fileStream);
    }

    /// <inheritdoc cref="ILogHandler.HandleMessage"/>
    public void HandleMessage(Message message)
    {
        _streamWriter.WriteLine(message.ToString());
        _streamWriter.Flush();
    }

    /// <summary>
    /// Disposes the file streams and flushes.
    /// </summary>
    public void Dispose()
    {
        _streamWriter.Flush();
        _streamWriter.Dispose();
        _fileStream.Dispose();
    }
}

/// <summary>
/// Symbolises a single log message.
/// </summary>
/// <param name="Text">The text of the message.</param>
/// <param name="Time">The time of the message.</param>
/// <param name="Severity">The severity of the message.</param>
/// <param name="FilePath">The path the message originated from.</param>
/// <param name="LineNumber">The line number the message originated from.</param>
public record Message(string Text, DateTime Time, Severity Severity, string FilePath, int LineNumber)
{
    public override string ToString()
    {
        return $"{Time.ToString("HH:mm:ss.ffff")} {Severity} \t {Path.GetFileName(FilePath)}#{LineNumber} \t| {Text}";
    }
}

/// <summary>
/// Handles logging in the game using <see cref="ILogHandler"/>s to handle the messages.
/// </summary>
public static class Log
{
    private static IEnumerable<ILogHandler>? _handlers;

    internal static void SetHandlers(IEnumerable<ILogHandler> handlers)
    {
        _handlers = handlers;
    }

    internal static void CloseHandlers()
    {
        if (_handlers is null)
        {
            return;
        }
        
        foreach (IDisposable handler  in _handlers.OfType<IDisposable>())
        {
            handler.Dispose();
        }
    }

    private static void HandleMessage(Severity severity, string text, string filePath, int lineNumber)
    {
        if (_handlers is null)
        {
            return;
        }

        Message message = new Message(text, DateTime.Now, severity, filePath, lineNumber);
        foreach (ILogHandler handler in _handlers)
        {
            handler.HandleMessage(message);
        }
    }

    /// <summary>
    /// Write a message with <see cref="Severity.Trace"/> level severity.
    /// </summary>
    /// <param name="message">The text of the message.</param>
    /// <param name="filePath">Auto filled, no need to provide this parameter.</param>
    /// <param name="lineNumber">Auto filled, no need to provide this parameter.</param>
    public static void Trace(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => HandleMessage(Severity.Trace, message, filePath, lineNumber);


    /// <summary>
    /// Write a message with <see cref="Severity.Debug"/> level severity.
    /// </summary>
    /// <param name="message">The text of the message.</param>
    /// <param name="filePath">Auto filled, no need to provide this parameter.</param>
    /// <param name="lineNumber">Auto filled, no need to provide this parameter.</param>
    public static void Debug(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => HandleMessage(Severity.Debug, message, filePath, lineNumber);


    /// <summary>
    /// Write a message with <see cref="Severity.Info"/> level severity.
    /// </summary>
    /// <param name="message">The text of the message.</param>
    /// <param name="filePath">Auto filled, no need to provide this parameter.</param>
    /// <param name="lineNumber">Auto filled, no need to provide this parameter.</param>
    public static void Info(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => HandleMessage(Severity.Info, message, filePath, lineNumber);


    /// <summary>
    /// Write a message with <see cref="Severity.Warn"/> level severity.
    /// </summary>
    /// <param name="message">The text of the message.</param>
    /// <param name="filePath">Auto filled, no need to provide this parameter.</param>
    /// <param name="lineNumber">Auto filled, no need to provide this parameter.</param>
    public static void Warn(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => HandleMessage(Severity.Warn, message, filePath, lineNumber);


    /// <summary>
    /// Write a message with <see cref="Severity.Error"/> level severity.
    /// </summary>
    /// <param name="message">The text of the message.</param>
    /// <param name="filePath">Auto filled, no need to provide this parameter.</param>
    /// <param name="lineNumber">Auto filled, no need to provide this parameter.</param>
    public static void Error(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => HandleMessage(Severity.Error, message, filePath, lineNumber);


    /// <summary>
    /// Write a message with <see cref="Severity.Fatal"/> level severity.
    /// </summary>
    /// <param name="message">The text of the message.</param>
    /// <param name="filePath">Auto filled, no need to provide this parameter.</param>
    /// <param name="lineNumber">Auto filled, no need to provide this parameter.</param>
    public static void Fatal(string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => HandleMessage(Severity.Fatal, message, filePath, lineNumber);

}