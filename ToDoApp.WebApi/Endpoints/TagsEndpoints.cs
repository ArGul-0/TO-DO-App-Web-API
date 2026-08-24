using System.Security.Claims;
using ToDoApp.Application.UseCases.Tags.GetAllMyTags;
using ToDoApp.Application.UseCases.Tags.GetMyTagById;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class TagsEndpoints
    {
        const string GetAllMyTagsEndpointName = "GetAllMyTags"; // Constant For The GetAllMyTags Endpoint Name
        const string GetMyTagByIdEndpointName = "GetMyTagById"; // Constant For The GetMyTagById Endpoint Name
        const string CreateNewTagEndpointName = "CreateNewTag"; // Constant For The CreateNewTag Endpoint Name
        const string UpdateUserTagEndpointName = "UpdateUserTag"; //Constant For The UpdateUserTag Endpoint Name
        const string DeleteUserTagEndpointName = "DeleteUserTag"; // Constant For The DeleteUserTag Endpoint Name

        public static RouteGroupBuilder MapTagsEndpoints(this WebApplication app)
        {
            var tagsGroup = app.MapGroup("/Tags"); // Create A Group For /Tags Endpoints


            tagsGroup.MapGet("/Me", async (GetAllMyTagsHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllMyTagsEndpointName).RequireAuthorization();

            tagsGroup.MapGet("/Me/{id}", async (int id, GetMyTagByIdHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(id, int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetMyTagByIdEndpointName).RequireAuthorization();

            tagsGroup.MapPost("/", async ( request, handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(request, int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.CreatedAtRoute(GetMyTagByIdEndpointName, new { id = result.Value.Id }, result.Value);
            }).WithName(CreateNewTagEndpointName).RequireAuthorization();

            tagsGroup.MapPut("/{tagId}", async (int tagId, request, handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(request, int.Parse(userId), tagId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(UpdateUserTagEndpointName).RequireAuthorization();

            tagsGroup.MapDelete("/{tagId}", async (int tagId, handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId), tagId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(DeleteUserTagEndpointName).RequireAuthorization();

            return tagsGroup;
        }
    }
}
