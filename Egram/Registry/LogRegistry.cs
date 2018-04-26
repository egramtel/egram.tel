using System.IO;
using Egram.Components.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace Egram.Registry
{
    public static class LogRegistry
    {
        public static void AddLog(this IServiceCollection services)
        {
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
            services.AddScoped(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton<ILoggerFactory>(p =>
            {
                var factory = new LoggerFactory();
                
                var options = new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                };
                factory.AddNLog(options);
                
                var config = new LoggingConfiguration();
            
                var target = new FileTarget("application");
                target.FileName = Path.Combine(p.GetService<Storage>().LogDirectory, "application.log");
                config.AddTarget(target);
            
                var rule = new LoggingRule("*", NLog.LogLevel.Debug, target);
                config.LoggingRules.Add(rule);
                factory.ConfigureNLog(config);
                
                return factory;
            });
        }
    }
}