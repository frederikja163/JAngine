using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace JAngine
{
    public enum Severity
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
    
    public record LogMessage(Severity Severity, int LineNumber, string FilePath, string MemberName, object Object)
    {
        public override string ToString()
        {
            return $"[{Severity} - {Path.GetFileNameWithoutExtension(FilePath)}#{LineNumber}] {Object}";
        }
    }

    public interface ILogger
    {
        public void Dispatch(LogMessage message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Dispatch(LogMessage message)
        {
            Console.WriteLine(message);
        }

        public static ConsoleColor GetColorFromSeverity(Severity severity) => severity switch
        {
            Severity.Debug => ConsoleColor.Gray,
            Severity.Info => ConsoleColor.White,
            Severity.Warn => ConsoleColor.Yellow,
            Severity.Error => ConsoleColor.Red,
            Severity.Fatal => ConsoleColor.DarkRed,
            _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
        };
    }

    public class FileLogger : ILogger, IDisposable
    {
        private StreamWriter _streamWriter;

        public FileLogger(string path)
        {
            _streamWriter = new StreamWriter(path);
        }
        
        public void Dispatch(LogMessage message)
        {
            _streamWriter.WriteLine(message);
        }

        public void Dispose()
        {
            _streamWriter.Dispose();
        }
    }
    
    public static class Log
    {
        private static readonly List<ILogger> _loggers = new List<ILogger>();

        public static void AddLogger(ILogger dispatch)
        {
            _loggers.Add(dispatch);
        }
        
        public static void Debug(object obj, [CallerLineNumber] int line = 0, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => SendMessage(new LogMessage(Severity.Debug, line, path, member, obj));
        public static void Info(object obj, [CallerLineNumber] int line = 0, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => SendMessage(new LogMessage(Severity.Info, line, path, member, obj));
        public static void Warn(object obj, [CallerLineNumber] int line = 0, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => SendMessage(new LogMessage(Severity.Warn, line, path, member, obj));
        public static void Error(object obj, [CallerLineNumber] int line = 0, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => SendMessage(new LogMessage(Severity.Error, line, path, member, obj));
        public static void Fatal(object obj, [CallerLineNumber] int line = 0, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => SendMessage(new LogMessage(Severity.Fatal, line, path, member, obj));

        private static void SendMessage(LogMessage logMessage)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.Dispatch(logMessage);
            }
        }

        internal static void Dispose()
        {
            foreach (ILogger logger in _loggers)
            {
                if (logger is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}