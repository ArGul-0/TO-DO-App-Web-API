using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.UseCases.Tags.CreateNewTag;
using ToDoApp.Application.UseCases.Tags.DeleteUserTag;
using ToDoApp.Application.UseCases.Tags.GetAllMyTags;
using ToDoApp.Application.UseCases.Tags.GetMyTagById;
using ToDoApp.Application.UseCases.Tags.UpdateUserTag;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Middlewares;

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
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllMyTagsEndpointName)
              .WithSummary("Get all tags belonging to the current user.")
              .RequireAuthorization()
              .Produces<List<TagDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound);

            tagsGroup.MapGet("/Me/{id}", async (int id, GetMyTagByIdHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(id, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetMyTagByIdEndpointName)
              .WithSummary("Get a tag by ID belonging to the current user.")
              .RequireAuthorization()
              .Produces<TagDto>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            tagsGroup.MapPost("/", async (CreateNewTagRequest request, CreateNewTagHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(request, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.CreatedAtRoute(GetMyTagByIdEndpointName, new { id = result.Value.Id }, result.Value);
            }).WithName(CreateNewTagEndpointName)
              .WithSummary("Create a tag for the current user.")
              .RequireAuthorization()
              .Produces<TagDto>(StatusCodes.Status201Created)
              .ProducesValidationProblem()
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound);

            tagsGroup.MapPut("/{tagId}", async (int tagId, UpdateUserTagRequest request, UpdateUserTagHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();
                
                var result = await handler.Handle(request, tagId, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(UpdateUserTagEndpointName)
              .WithSummary("Update a tag belonging to the current user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .ProducesValidationProblem()
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            tagsGroup.MapDelete("/{tagId}", async (int tagId, DeleteUserTagHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(tagId, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(DeleteUserTagEndpointName)
              .WithSummary("Delete a tag belonging to the current user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            return tagsGroup;
        }
    }
}
