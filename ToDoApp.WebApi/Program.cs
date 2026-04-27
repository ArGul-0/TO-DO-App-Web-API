using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using ToDoApp.Application.Interfaces;
using ToDoApp.Infrastructure.Authentication.Jwt;
using ToDoApp.Infrastructure.Authentication.Password;
using ToDoApp.Infrastructure.DependencyInjection;
using ToDoApp.WebApi.Endpoints;
using ToDoApp.WebApi.Extensions;
using ToDoApp.Infrastructure.Repositories;

namespace ToDoApp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.AddSerilogLogging();
            builder.AddSwagger();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

            builder.AddAuth();

            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IPasswordHasher, Argon2Hasher>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

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
                    MinimumSameSitePolicy = SameSiteMode.Strict // Set SameSite Policy To Strict To Prevent CSRF Attacks
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAuthEndpoints();

            app.Run();
        }
    }
}
