using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.Application.UseCases.Users.GetUserById;
using ToDoApp.Application.UseCases.Users.LoginUser;
using ToDoApp.Infrastructure;
using ToDoApp.Infrastructure.Authentication.Jwt;
using ToDoApp.Infrastructure.Authentication.Password;
using ToDoApp.Infrastructure.DependencyInjection;
using ToDoApp.Infrastructure.Repositories;
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
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<CreateUserHandler>();
            builder.Services.AddScoped<LoginUserHandler>();
            builder.Services.AddScoped<GetAllUsersHandler>();
            builder.Services.AddScoped<GetUserByIdHandler>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddValidation(); // Add Validation Services

            var app = builder.Build();

            app.UseHttpsRedirection();

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
                    MinimumSameSitePolicy = SameSiteMode.Strict // Set SameSite Policy To Strict To Prevent CSRF Attacks
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>(); // Global Exception Handling Middleware

            app.MapAuthEndpoints();
            app.MapUsersEndpoints();
            app.MapNotesEndpoints();

            app.MigrateDatabase(); // Apply Database Migrations On Startup

            app.Run();
        }
    }
}
