using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Application.UseCases.Users.LoginUser;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        const string RegisterEndpointName = "Register"; // Constant For The Register Endpoint Name
        const string LoginEndpointName = "Login"; // Constant For The Login Endpoint Name

        public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/Auth"); // Create A Group For /Auth Endpoints



            authGroup.MapPost("/Register", async (
                CreateUserRequest request,
                CreateUserHandler createUserHandler,
                HttpContext httpContext,
                IConfiguration configuration) =>
            {
                var result = await createUserHandler.Handle(request);

                if (result.IsFailure)
                    return result.ToHttpResult();

                httpContext.Response.Cookies
                .Append(configuration["JwtOptions:NameInCookies"]!, result.Value.ToString()); // Set The JWT Token In Cookies

                return Results.Ok(result.Value);
            }).WithName(RegisterEndpointName);

            authGroup.MapPost("/Login", async (
                LoginUserRequest request,
                LoginUserHandler loginUserHandler,
                HttpContext httpContext,
                IConfiguration configuration) =>
            {
                var result = await loginUserHandler.Handle(request);
                if (result.IsFailure)
                    return result.ToHttpResult();

                httpContext.Response.Cookies
                .Append(configuration["JwtOptions:NameInCookies"]!, result.Value.ToString()); // Set The JWT Token In Cookies

                return Results.Ok(result.Value);
            }).WithName(LoginEndpointName);

            return authGroup;
        }
    }
}
