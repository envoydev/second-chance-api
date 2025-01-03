using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace SecondChance.Infrastructure.Logger.Formatters;

public class SerilogJsonFormatter : ITextFormatter
{
    private readonly JsonValueFormatter _valueFormatter;

    public SerilogJsonFormatter(JsonValueFormatter? valueFormatter = null)
    {
        _valueFormatter = valueFormatter ?? new JsonValueFormatter("$type");
    }

    public void Format(LogEvent logEvent, TextWriter output)
    {
        CompactJsonFormatter.FormatEvent(logEvent, output, _valueFormatter);
        output.Write(Environment.NewLine);
    }
}