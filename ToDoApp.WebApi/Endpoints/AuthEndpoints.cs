using ToDoApp.Application.UseCases.Users.CreateUser;

namespace ToDoApp.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/Auth"); // Create A Group For /Auth Endpoints

            authGroup.MapPost("/Test", (CreateUserHandler createUserHandler) =>
            {

            }).WithName("TestAuthEndpoint");

            return authGroup;
        }
    }
}
