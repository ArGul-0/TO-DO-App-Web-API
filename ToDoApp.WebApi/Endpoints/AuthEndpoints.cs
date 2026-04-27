namespace ToDoApp.WebApi.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/Auth"); // Create A Group For /Auth Endpoints

            authGroup.MapGet("/Test", () =>
            {
                return Results.Ok(new { Message = "Auth Endpoint Is Working!" });
            });

            return authGroup;
        }
    }
}
