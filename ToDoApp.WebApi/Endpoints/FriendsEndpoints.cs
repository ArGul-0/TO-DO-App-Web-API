using System.Security.Claims;
using ToDoApp.Application.UseCases.Friends.AcceptFriendRequest;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests;
using ToDoApp.Application.UseCases.Friends.RejectFriendRequest;
using ToDoApp.Application.UseCases.Friends.RemoveFriendship;
using ToDoApp.Application.UseCases.Friends.SendFriendRequest;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class FriendsEndpoints
    {
        const string GetAllMyFriendsEndpointName = "GetAllMyFriends"; // Constant For The GetAllMyFriends Endpoint Name
        const string GetIncomingFriendshipRequestsEndpointName = "GetIncomingFriendshipRequests"; // Constant For The GetIncomingFriendshipRequests Endpoint Name
        const string SendFriendRequestEndpointName = "SendFriendRequest"; // Constant For The SendFriendRequest Endpoint Name
        const string AcceptFriendRequestEndpointName = "AcceptFriendRequest"; // Constant For The AcceptFriendRequest Endpoint Name
        const string RejectFriendRequestEndpointName = "RejectFriendRequest"; // Constant For The RejectFriendRequest Endpoint Name
        const string RemoveFriendshipEndpointName = "RemoveFriendship"; // Constant For The RemoveFriendship Endpoint Name

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

            friendsGroup.MapPost("/Requests/{friendId}", async (int friendId, SendFriendshipRequestHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId), friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Created();
            }).WithName(SendFriendRequestEndpointName).RequireAuthorization();

            friendsGroup.MapPost("/Requests/{friendId}/Accept", async (int friendId, AcceptFriendshipRequestHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId), friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();
            }).WithName(AcceptFriendRequestEndpointName).RequireAuthorization();

            friendsGroup.MapPost("/Requests/{friendId}/Reject", async (int friendId, RejectFriendshipRequestHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId), friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();
            }).WithName(RejectFriendRequestEndpointName).RequireAuthorization();

            friendsGroup.MapDelete("/{friendId}", async (int friendId, RemoveFriendshipHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId), friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();
            }).WithName(RemoveFriendshipEndpointName).RequireAuthorization();

            return friendsGroup;
        }
    }
}
