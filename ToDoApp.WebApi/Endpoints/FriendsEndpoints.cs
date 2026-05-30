using System.Security.Claims;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class FriendsEndpoints
    {
        const string GetAllMyFriendsEndpointName = "GetAllMyFriends"; // Constant For The GetAllMyFriends Endpoint Name
        const string GetIncomingFriendshipRequestsEndpointName = "GetIncomingFriendshipRequests"; // Constant For The GetIncomingFriendshipRequests Endpoint Name

        public static RouteGroupBuilder MapFriendsEndpoints(this WebApplication app)
        {
            var friendsGroup = app.MapGroup("/Friends"); // Create A Group For /Friends Endpoints

            friendsGroup.MapGet("/", async (GetAllMyFriendshipsHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllMyFriendsEndpointName).RequireAuthorization();

            friendsGroup.MapGet("/Incoming", async (GetIncomingFriendshipRequestsHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetIncomingFriendshipRequestsEndpointName).RequireAuthorization();

            return friendsGroup;
        }
    }
}
