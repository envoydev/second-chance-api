using Serilog.Core;
using Serilog.Events;

namespace SecondChance.Infrastructure.Logger.Enrichers;

internal class TaskIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var taskId = Task.CurrentId ?? -1;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TaskId", taskId));
    }
}