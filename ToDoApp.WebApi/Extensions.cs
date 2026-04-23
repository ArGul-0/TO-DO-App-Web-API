using Serilog;
using Serilog.Events;

namespace ToDoApp.WebApi
{
    public static class Extensions
    {
        /// <summary>
        /// Configures Serilog as the logging provider for the specified web application builder with default enrichers
        /// and sinks.
        /// </summary>
        /// <remarks>This method sets up Serilog with console and Seq sinks, enriches log events with
        /// environment, process, and thread information, and overrides the minimum log level for Microsoft components
        /// to Warning. Call this method early in the application's startup to ensure logging is configured before other
        /// services are added.</remarks>
        /// <param name="builder">The web application builder to configure with Serilog logging. Cannot be null.</param>
        /// <returns>The same instance of <see cref="WebApplicationBuilder"/> with Serilog logging configured.</returns>
        public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }
    }
}
