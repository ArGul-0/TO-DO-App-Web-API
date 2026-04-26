using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using ToDoApp.Application.Interfaces;
using ToDoApp.Infrastructure.Authentication;
using ToDoApp.Infrastructure.DependencyInjection;
using ToDoApp.WebApi.Endpoints;
using ToDoApp.WebApi.Extensions;

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

            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAuthEndpoints();

            app.Run();
        }
    }
}
