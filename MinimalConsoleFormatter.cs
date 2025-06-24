using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;

namespace SkyQuant;

public class MinimalConsoleFormatter : ConsoleFormatter
{
    public MinimalConsoleFormatter() : base("minimal") { }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider scopeProvider,
        TextWriter textWriter)
    {
        var logLevel = logEntry.LogLevel;
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);

        if (message == null)
            return;

        textWriter.WriteLine(message);
    }
}
