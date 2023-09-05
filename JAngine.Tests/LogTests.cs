using JAngine.Core;
using NUnit.Framework;
using ILogger = JAngine.Core.ILogger;

namespace JAngine.Tests;

public class LogTests
{
    public class TestLogger : ILogger
    {
        public static List<LogMessage> Data = new();
        public void HandleMessage(LogMessage message)
        {
            Data.Add(message);
        }
    }

    [TestCase(5, Severity.Info)]
    [TestCase(546, Severity.Debug)]
    [TestCase(100543, Severity.Warning)]
    [TestCase(112345654, Severity.Error)]
    public void CustomLoggerTests(object message, Severity severity)
    {
        Log.Message(severity, message);
        Assert.That(severity, Is.EqualTo(TestLogger.Data[^1].Severity));
    }

    [TestCase(5, Severity.Info)]
    [TestCase(546, Severity.Debug)]
    [TestCase(100543, Severity.Warning)]
    [TestCase(112345654, Severity.Error)]
    public void MessageEqualsSpecificLog(object message, Severity severity)
    {
        Log.Message(severity, message);
        switch (severity)
        {
            case Severity.Debug:
                Log.Debug(message);
                break;
            case Severity.Info:
                Log.Info(message);
                break;
            case Severity.Warning:
                Log.Warning(message);
                break;
            case Severity.Error:
                Log.Error(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
        }
        Assert.That(TestLogger.Data[^1].Severity, Is.EqualTo(TestLogger.Data[^2].Severity));
        Assert.That(TestLogger.Data[^1].Data, Is.EqualTo(TestLogger.Data[^2].Data));
    }
}