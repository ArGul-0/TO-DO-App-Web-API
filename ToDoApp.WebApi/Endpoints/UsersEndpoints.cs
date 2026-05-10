using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoApp.Application.UseCases.Users.ChangeUserVisibility;
using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.Application.UseCases.Users.GetUserById;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class UsersEndpoints
    {
        const string GetAllUsersEndpointName = "GetAllUsers"; // Constant For The GetAllUsers Endpoint Name
        const string GetUserByIdEndpointName = "GetUserById"; // Constant For The GetUserById Endpoint Name
        const string ChangeUserVisibilityEndpointName = "ChangeUserVisibility"; // Constant For The ChangeUserVisibility Endpoint Name

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

            usersGroup.MapGet("/{id}", async (GetUserByIdHandler handler, int id) =>
            {
                var result = await handler.Handle(id);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetUserByIdEndpointName);

            usersGroup.MapPost("/Me/ChangeUserVisibility", async (ChangeUserVisibilityRequest request, ChangeUserVisibilityHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(request, int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();

            }).WithName(ChangeUserVisibilityEndpointName).RequireAuthorization();

            return usersGroup;
        }
    }
}
