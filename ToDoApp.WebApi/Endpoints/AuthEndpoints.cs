using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        const string LoginEndpointName = "Login"; // Constant For The Login Endpoint Name
        const string RegisterEndpointName = "Register"; // Constant For The Register Endpoint Name

        public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/Auth"); // Create A Group For /Auth Endpoints



            authGroup.MapPost("/Test", async (
                CreateUserHandler createUserHandler,
                HttpContext httpContext,
                IConfiguration configuration) =>
            {
                var result = await createUserHandler.Handle(new CreateUserRequest(
                    Username: "testuser",
                    Email: "testEmail@gmail.com",
                    Password: "TestPassword123"
                    ));

                if (result.IsFailure)
                    return result.ToHttpResult();

                httpContext.Response.Cookies
                .Append(configuration["JwtOptions:NameInCookies"]!, result.Value.ToString()); // Set The JWT Token In Cookies

                return Results.Ok(result.Value);
            }).WithName("TestAuthEndpoint");

            return authGroup;
        }
    }
}
