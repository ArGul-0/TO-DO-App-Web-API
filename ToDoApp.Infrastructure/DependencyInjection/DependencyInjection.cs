using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Infrastructure.Authentication.Jwt;
using ToDoApp.Infrastructure.Authentication.Password;
using ToDoApp.Infrastructure.Repositories;

namespace ToDoApp.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application infrastructure services, including the database context and related dependencies, to the
        /// specified service collection.
        /// </summary>
        /// <remarks>This method configures the application's database context using the connection string
        /// named "Default" from the provided configuration. It also registers the database context and its abstraction
        /// for dependency injection.</remarks>
        /// <param name="services">The service collection to which the infrastructure services will be added.</param>
        /// <param name="configuration">The application configuration used to retrieve connection settings.</param>
        /// <returns>The updated service collection with infrastructure services registered.</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is missing or empty.");
            }

            try
            {
                _ = new NpgsqlConnectionStringBuilder(connectionString);
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is invalid.", exception);
            }

            services.AddOptions<JwtOptions>()
                .Bind(configuration.GetSection("JwtOptions"))
                .Validate(options => !string.IsNullOrWhiteSpace(options.SecretKey), "JwtOptions:SecretKey is required.")
                .Validate(options => Encoding.UTF8.GetByteCount(options.SecretKey ?? string.Empty) >= 32, "JwtOptions:SecretKey must be at least 32 bytes for HS256.")
                .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "JwtOptions:Issuer is required.")
                .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "JwtOptions:Audience is required.")
                .Validate(options => options.ExpirationHours > 0, "JwtOptions:ExpirationHours must be greater than zero.")
                .Validate(options => !string.IsNullOrWhiteSpace(options.NameInCookies), "JwtOptions:NameInCookies is required.")
                .ValidateOnStart();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, Argon2Hasher>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static void MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
