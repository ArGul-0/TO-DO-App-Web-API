using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class UsersEndpoints
    {
        const string GetAllUsersEndpointName = "GetAllUsers"; // Constant For The GetAllUsers Endpoint Name

        public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
        {
            var usersGroup = app.MapGroup("/Users"); // Create A Group For /Users Endpoints

            usersGroup.MapGet("/", async (GetAllUsersHandler handler) =>
            {
                var result = await handler.Handle();

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllUsersEndpointName);

            return usersGroup;
        }
    }
}
