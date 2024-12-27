using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecondChance.Infrastructure.Logger;
using Serilog.Extensions.Logging;

namespace SecondChance.Infrastructure.Extensions;

internal static class LoggerBuilderExtensions
{
    public static void AddSerilogLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();

        loggingBuilder.Services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>(serviceProvider =>
        {
            var serilogLoggerConfiguration = serviceProvider.GetRequiredService<SerilogLoggerConfiguration>();

            return new SerilogLoggerProvider(serilogLoggerConfiguration.CreateConfiguration(), true);
        });

        loggingBuilder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
    }
}