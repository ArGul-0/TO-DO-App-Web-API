using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application.Interfaces;

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
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

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
