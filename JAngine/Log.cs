namespace JAngine;

public static class Log
{
    public enum Severity
    {
        Debug,
        Trace,
        Info,
        Warning,
        Error,
    }

    private static void LogMessage(Severity severity, object[] objs)
    {
        // TODO: Allow writing to multiple different places here, i.e. both the console and files.
        // TODO: Write the stack trace as well.
        Console.WriteLine($"{severity.ToString()} | {string.Join(", ", objs)}");
    }

    public static void Info(params object[] objs)
        => LogMessage(Severity.Info, objs);
    public static void Warning(params object[] objs)
        => LogMessage(Severity.Warning, objs);
    public static void Error(params object[] objs)
        => LogMessage(Severity.Error, objs);

    public static void Debug(params object[] objs)
    {
        if (Engine.Config.Debug)
        {
            LogMessage(Severity.Debug, objs);
        }
    }
    public static void Trace(params object[] objs)
    {
        if (Engine.Config.Trace)
        {
            LogMessage(Severity.Trace, objs);
        }
    }
}