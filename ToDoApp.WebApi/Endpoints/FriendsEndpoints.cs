using System.Security.Claims;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class FriendsEndpoints
    {
        const string GetAllMyFriendsEndpointName = "GetAllMyFriends"; // Constant For The GetAllMyFriends Endpoint Name

        public static RouteGroupBuilder MapFriendsEndpoints(this WebApplication app)
        {
            var friendsGroup = app.MapGroup("/Friends"); // Create A Group For /Friends Endpoints

            friendsGroup.MapGet("/Me/", async (GetAllMyFriendshipsHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllMyFriendsEndpointName).RequireAuthorization();

            return friendsGroup;
        }
    }
}
