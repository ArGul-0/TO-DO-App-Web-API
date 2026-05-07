using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Application.UseCases.Users.LoginUser;

namespace ToDoApp.WebApi.Endpoints
{
    public static class UsersEndpoints
    {
        const string GetAllUsersEndpointName = "GetAllUsers"; // Constant For The GetAllUsers Endpoint Name

        public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
        {
            var usersGroup = app.MapGroup("/Users"); // Create A Group For /Users Endpoints

            return usersGroup;
        }
    }
}
