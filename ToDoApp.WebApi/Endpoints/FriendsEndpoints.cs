using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.UseCases.Friends.AcceptFriendRequest;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests;
using ToDoApp.Application.UseCases.Friends.RejectFriendRequest;
using ToDoApp.Application.UseCases.Friends.RemoveFriendship;
using ToDoApp.Application.UseCases.Friends.SendFriendRequest;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Middlewares;

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
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllMyFriendsEndpointName)
              .WithSummary("Get all friendships for the current user.")
              .RequireAuthorization()
              .Produces<List<FriendshipDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError);

            friendsGroup.MapGet("/Incoming", async (GetIncomingFriendshipRequestsHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetIncomingFriendshipRequestsEndpointName)
              .WithSummary("Get incoming friendship requests for the current user.")
              .RequireAuthorization()
              .Produces<List<FriendshipDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError);

            friendsGroup.MapPost("/Requests/{friendId}", async (int friendId, SendFriendshipRequestHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId, friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Created();
            }).WithName(SendFriendRequestEndpointName)
              .WithSummary("Send a friendship request to the specified user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status201Created)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Conflict);

            friendsGroup.MapPost("/Requests/{friendId}/Accept", async (int friendId, AcceptFriendshipRequestHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId, friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();
            }).WithName(AcceptFriendRequestEndpointName)
              .WithSummary("Accept the friendship request from the specified user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status200OK)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Unauthorized);

            friendsGroup.MapPost("/Requests/{friendId}/Reject", async (int friendId, RejectFriendshipRequestHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId, friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();
            }).WithName(RejectFriendRequestEndpointName)
              .WithSummary("Reject the friendship request from the specified user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status200OK)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Unauthorized);

            friendsGroup.MapDelete("/{friendId}", async (int friendId, DeleteFriendship handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId, friendId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(RemoveFriendshipEndpointName)
              .WithSummary("Remove the friendship with the specified user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Conflict, ErrorType.Unauthorized);

            return friendsGroup;
        }
    }
}
