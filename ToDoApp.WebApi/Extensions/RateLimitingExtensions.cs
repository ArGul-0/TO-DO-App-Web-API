using System.Threading.RateLimiting;

namespace ToDoApp.WebApi.Extensions
{
    public static class RateLimitingExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public WebApplicationBuilder AddRateLimiting()
            {
                builder.Services.AddRateLimiter(options =>
                {
                    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                    options.AddPolicy("AuthLimit", context =>
                    {
                        var ipAddress = context.Connection.RemoteIpAddress;
                        var partitionKey = ipAddress?.ToString() ?? "unknown";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey,
                            _ => new FixedWindowRateLimiterOptions
                            {
                                Window = TimeSpan.FromMinutes(1),
                                PermitLimit = 10,
                                QueueLimit = 0,
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                AutoReplenishment = true
                            });
                    });
                });

                return builder;
            }
        }
    }
}
