using Serilog;
using Serilog.Events;

namespace ToDoApp.WebApi.Extensions
{
    public static class SerilogExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            /// <summary>
            /// Configures Serilog as the logging provider for the specified web application builder with default enrichers
            /// and sinks.
            /// </summary>
            /// <remarks>This method sets up Serilog with console and Seq sinks, enriches log events with
            /// environment, process, and thread information, and overrides the minimum log level for Microsoft components
            /// to Warning. Call this method early in the application's startup to ensure logging is configured before other
            /// services are added.</remarks>
            /// <returns>The same instance of <see cref="WebApplicationBuilder"/> with Serilog logging configured.</returns>
            public WebApplicationBuilder AddSerilogLogging()
            {
                var seqUrl = builder.Configuration["Serilog:SeqUrl"] ?? throw new NullReferenceException("Serilog:SeqUrl configuration is missing.");

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithProcessId()
                    .Enrich.WithThreadId()
                    .WriteTo.Console()
                    .WriteTo.Seq(seqUrl)
                    .CreateLogger();

                builder.Host.UseSerilog();

                return builder;
            }
        }
    }
}
