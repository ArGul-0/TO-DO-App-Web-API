using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Events;
using System.Text;
using ToDoApp.Infrastructure.Authentication;

namespace ToDoApp.WebApi.Extensions
{
    public static class ApiExtensions
    {
        /// <summary>
        /// Configures Swagger services for the specified web application builder with a custom API documentation setup.
        /// </summary>
        /// <remarks>This method sets up Swagger to generate API documentation with a title, version, description, and contact information.<remarks>
        /// <param name="builder">The web application builder to configure with Swagger services. Cannot be null.</param>
        /// <returns>The same instance of <see cref="WebApplicationBuilder"/> with Swagger services configured.</returns>
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TO-DO App Web API",
                    Description = "An ASP.NET Core Web API for managing TO-DO items",
                    Contact = new OpenApiContact
                    {
                        Name = "ArGul",
                    }
                });
            });

            return builder;
        }

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

        public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Add Authentication Services With JWT Bearer Scheme
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>() ?? throw new InvalidOperationException("JwtOptions configuration is missing.");

                    options.TokenValidationParameters = new TokenValidationParameters // Configure Token Validation Parameters
                    {
                        ValidateIssuer = true, // Enable Issuer Validation To Ensure Token Is Issued By A Trusted Authority
                        ValidIssuer = jwtOptions.Issuer, // Set The Valid Issuer To The Value From Configuration
                        ValidateAudience = true, // Enable Audience Validation To Ensure Token Is Intended For This API
                        ValidAudience = jwtOptions.Audience, // Set The Valid Audience To The Value From Configuration
                        ValidateLifetime = true, // Enable Lifetime Validation To Ensure Tokens Expire
                        ClockSkew = TimeSpan.Zero, // Set Clock Skew To Zero To Prevent Delayed Expiration Issues
                        ValidateIssuerSigningKey = true, // Enable Issuer Signing Key Validation To Ensure Token Integrity
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            jwtOptions.SecretKey!)), // Use A Symmetric Security Key Derived From The Secret Key In Configuration
                    };

                    options.Events = new JwtBearerEvents // Configure JWT Bearer Events For Better Error Handling
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies[jwtOptions.NameInCookies!];

                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
