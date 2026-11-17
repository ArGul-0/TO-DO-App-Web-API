using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoApp.Infrastructure.Authentication.Jwt;

namespace ToDoApp.WebApi.Extensions
{
    public static class AuthenticationExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public WebApplicationBuilder AddAuth()
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>() ?? throw new InvalidOperationException("JwtOptions configuration is missing.");

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtOptions.Issuer,
                            ValidateAudience = true,
                            ValidAudience = jwtOptions.Audience,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                jwtOptions.SecretKey!)),
                        };

                        options.Events = new JwtBearerEvents
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
}
