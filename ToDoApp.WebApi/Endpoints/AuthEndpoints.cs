using ToDoApp.Application.Common;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Application.UseCases.Users.LoginUser;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Middlewares;

namespace ToDoApp.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        const string RegisterEndpointName = "Register"; // Constant For The Register Endpoint Name
        const string LoginEndpointName = "Login"; // Constant For The Login Endpoint Name
        const string LogoutEndpointName = "Logout"; // Constant For The Logout Endpoint Name

        public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/Auth").WithTags("Auth"); // Create A Group For /Auth Endpoints



            authGroup.MapPost("/Register", async (
                CreateUserRequest request,
                CreateUserHandler createUserHandler,
                HttpContext httpContext,
                IConfiguration configuration) =>
            {
                var result = await createUserHandler.Handle(request);

                if (result.IsFailure)
                    return result.ToHttpResult();

                var jwtExpires = DateTimeOffset.UtcNow.AddHours(
                    configuration.GetValue<int>("JwtOptions:ExpirationHours"));

                httpContext.Response.Cookies
                .Append(configuration["JwtOptions:NameInCookies"] ?? throw new InvalidOperationException("Jwt cookie name not configured"),
                result.Value.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = jwtExpires
                }); // Set The JWT Token In Cookies

                return Results.Ok(result.Value);
            }).WithName(RegisterEndpointName)
              .WithSummary("Register a new user and issue an authentication cookie.")
              .RequireRateLimiting("AuthLimit")
              .Produces<CreateUserResponse>(StatusCodes.Status200OK)
              .ProducesValidationProblem()
              .Produces(StatusCodes.Status429TooManyRequests)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.Conflict);

            authGroup.MapPost("/Login", async (
                LoginUserRequest request,
                LoginUserHandler loginUserHandler,
                HttpContext httpContext,
                IConfiguration configuration) =>
            {
                var result = await loginUserHandler.Handle(request);
                if (result.IsFailure)
                    return result.ToHttpResult();

                var jwtExpires = DateTimeOffset.UtcNow.AddHours(
                    configuration.GetValue<int>("JwtOptions:ExpirationHours"));

                httpContext.Response.Cookies
                .Append(configuration["JwtOptions:NameInCookies"] ?? throw new InvalidOperationException("Jwt cookie name not configured"),
                result.Value.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = jwtExpires
                }); // Set The JWT Token In Cookies

                return Results.Ok(result.Value);
            }).WithName(LoginEndpointName)
              .WithSummary("Authenticate a user and issue an authentication cookie.")
              .RequireRateLimiting("AuthLimit")
              .Produces<LoginUserResponse>(StatusCodes.Status200OK)
              .ProducesValidationProblem()
              .Produces(StatusCodes.Status429TooManyRequests)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Unauthorized);

            authGroup.MapPost("/Logout", async (
                HttpContext httpContext,
                IConfiguration configuration) =>
            {
                httpContext.Response.Cookies.Delete(configuration["JwtOptions:NameInCookies"] ?? throw new InvalidOperationException("Jwt cookie name not configured"));
                
                return Results.Ok();
            }).WithName(LogoutEndpointName)
              .WithSummary("Log out the current user by deleting the authentication cookie.")
              .Produces(StatusCodes.Status200OK)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError);

            return authGroup;
        }
    }
}
