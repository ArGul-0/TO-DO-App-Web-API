using Microsoft.OpenApi;
using System.Reflection;

namespace ToDoApp.WebApi.Extensions
{
    public static class SwaggerExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            /// <summary>
            /// Configures Swagger services for the specified web application builder with a custom API documentation setup.
            /// </summary>
            /// <remarks>This method sets up Swagger to generate API documentation with a title, version, description, and contact information.</remarks>
            /// <returns>The same instance of <see cref="WebApplicationBuilder"/> with Swagger services configured.</returns>
            public WebApplicationBuilder AddSwagger()
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

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    options.IncludeXmlComments(xmlPath);
                });

                return builder;
            }
        }
    }
}
