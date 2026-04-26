namespace ToDoApp.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/Auth"); // Create A Group For /Auth Endpoints

            authGroup.MapGet("/Test", () => "Auth Endpoint Is Working!").WithTags("Auth"); // Example Test Endpoint

            return authGroup;
        }

    }
}
