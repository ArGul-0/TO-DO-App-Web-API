using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using ToDoApp.Application;
using ToDoApp.Infrastructure.DependencyInjection;
using ToDoApp.WebApi.Endpoints;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Middlewares;

namespace ToDoApp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);
            builder.AddSerilogLogging();
            builder.AddSwagger();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.AddAuth();
            builder.AddRateLimiting();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddValidation(); // Add Validation Services

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRateLimiter();

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseSerilogRequestLogging(); // Enable Serilog Request Logging

            app.MapOpenApi();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseCookiePolicy(
                new CookiePolicyOptions
                {
                    HttpOnly = HttpOnlyPolicy.Always, // Ensure Cookies Are Marked As HttpOnly To Prevent Client-Side Access
                    Secure = CookieSecurePolicy.Always, // Ensure Cookies Are Only Sent Over HTTPS
                    MinimumSameSitePolicy = SameSiteMode.Lax // Set SameSite Policy To Lax To Prevent CSRF Attacks
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>(); // Global Exception Handling Middleware

            app.MapAuthEndpoints();
            app.MapUsersEndpoints();
            app.MapNotesEndpoints();
            app.MapTagsEndpoints();
            app.MapFriendsEndpoints();

            app.MigrateDatabase(); // Apply Database Migrations On Startup

            app.MapGet("/health", async () => Results.Ok("Healthy"))
                .WithName("HealthCheck")
                .WithSummary("Check whether the API is healthy."); // Health Check Endpoint

            app.Run();
        }
    }
}
