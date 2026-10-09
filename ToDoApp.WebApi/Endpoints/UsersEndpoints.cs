using System.Security.Claims;
using ToDoApp.Application.UseCases.Users.ChangeUserVisibility;
using ToDoApp.Application.UseCases.Users.DeleteMyAccount;
using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.Application.UseCases.Users.GetCurrentUser;
using ToDoApp.Application.UseCases.Users.GetUserById;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class UsersEndpoints
    {
        const string GetAllUsersEndpointName = "GetAllUsers"; // Constant For The GetAllUsers Endpoint Name
        const string GetUserByIdEndpointName = "GetUserById"; // Constant For The GetUserById Endpoint Name
        const string GetCurrentUserEndpointName = "GetCurrentUser"; // Constant For The GetCurrentUser Endpoint Name
        const string ChangeUserVisibilityEndpointName = "ChangeUserVisibility"; // Constant For The ChangeUserVisibility Endpoint Name
        const string DeleteMyAccountEndpointName = "DeleteMyAccount"; // Constant For The DeleteMyAccount Endpoint Name

        public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
        {
            var usersGroup = app.MapGroup("/Users"); // Create A Group For /Users Endpoints

            usersGroup.MapGet("/", async (GetAllUsersHandler handler) =>
            {
                var result = await handler.Handle();

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllUsersEndpointName)
              .WithSummary("Get all users.");

            usersGroup.MapGet("/{id}", async (GetUserByIdHandler handler, int id) =>
            {
                var result = await handler.Handle(id);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetUserByIdEndpointName)
              .WithSummary("Get a user by ID.");

            usersGroup.MapGet("/Me", async (GetCurrentUserHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetCurrentUserEndpointName)
              .WithSummary("Get the current user's profile.")
              .RequireAuthorization();

            usersGroup.MapPost("/Me/ChangeUserVisibility", async (ChangeUserVisibilityRequest request, ChangeUserVisibilityHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(request, int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();

            }).WithName(ChangeUserVisibilityEndpointName)
              .WithSummary("Change the visibility setting for the current user's profile.")
              .RequireAuthorization();

            usersGroup.MapDelete("/Me", async (DeleteMyAccountHandler handler, IConfiguration configuration, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                context.Response.Cookies.Delete(configuration["JwtOptions:NameInCookies"] ?? throw new InvalidOperationException("Jwt cookie name not configured"));

                return Results.Ok();
            }).WithName(DeleteMyAccountEndpointName)
              .WithSummary("Delete the current user's account and clear the authentication cookie.")
              .RequireAuthorization();

            return usersGroup;
        }
    }
}
