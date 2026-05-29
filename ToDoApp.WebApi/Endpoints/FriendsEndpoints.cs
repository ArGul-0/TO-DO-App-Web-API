namespace ToDoApp.WebApi.Endpoints
{
    public static class FriendsEndpoints
    {
        public static RouteGroupBuilder MapFriendsEndpoints(this WebApplication app)
        {
            var friendsGroup = app.MapGroup("/Friends"); // Create A Group For /Friends Endpoints

            return friendsGroup;
        }
    }
}
