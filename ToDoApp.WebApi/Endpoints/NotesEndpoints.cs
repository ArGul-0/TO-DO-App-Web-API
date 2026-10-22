using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.UseCases.Notes.AttachTagToNote;
using ToDoApp.Application.UseCases.Notes.CreateNewNote;
using ToDoApp.Application.UseCases.Notes.DeleteUserNote;
using ToDoApp.Application.UseCases.Notes.DetachTagFromNote;
using ToDoApp.Application.UseCases.Notes.GetAllNotes;
using ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes;
using ToDoApp.Application.UseCases.Notes.GetNoteById;
using ToDoApp.Application.UseCases.Notes.GetUserNotesByTag;
using ToDoApp.Application.UseCases.Notes.UpdateUserNote;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Middlewares;

namespace ToDoApp.WebApi.Endpoints
{
    public static class NotesEndpoints
    {
        const string GetAllNotesEndpointName = "GetAllNotes"; // Constant For The GetAllNotes Endpoint Name
        const string GetNoteByIdEndpointName = "GetNoteById"; // Constant For The GetNoteById Endpoint Name
        const string GetAllUserNotesEndpointName = "GetAllUserNotes"; // Constant For The GetAllUserNotes Endpoint Name
        const string GetUserNotesByTagEndpointName = "GetUserNotesByTag"; // Constant For The GetUserNotesByTag Endpoint Name
        const string CreateNewNoteEndpointName = "CreateNewNote"; // Constant For The CreateNewNote Endpoint Name Name
        const string UpdateUserNoteEndpointName = "UpdateUserNote"; // Constant For The UpdateUserNote Endpoint Name
        const string DeleteUserNoteEndpointName = "DeleteUserNote"; // Constant For The DeleteUserNote Endpoint Name
        const string AttachTagToNoteEndpointName = "AttachTagToNote"; // Constant For The AttachTagToNote Endpoint Name
        const string DetachTagFromNoteEndpointName = "DetachTagFromNote"; // Constant For The DetachTagFromNote Endpoint

        public static RouteGroupBuilder MapNotesEndpoints(this WebApplication app)
        {
            var notesGroup = app.MapGroup("/Notes").WithTags("Notes"); // Create A Group For /Notes Endpoints

            notesGroup.MapGet("/", async (GetAllNotesHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllNotesEndpointName)
              .WithSummary("Get notes visible to the current user.")
              .RequireAuthorization()
              .Produces<List<NoteDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound);

            notesGroup.MapGet("/{noteId}", async (int noteId, GetNoteByIdHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(noteId, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetNoteByIdEndpointName)
              .WithSummary("Get a note by ID if it is visible to the current user.")
              .RequireAuthorization()
              .Produces<NoteDto>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            notesGroup.MapGet("/Me", async (GetAllUserNotesHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllUserNotesEndpointName)
              .WithSummary("Get all notes created by the current user.")
              .RequireAuthorization()
              .Produces<List<NoteDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError);

            notesGroup.MapGet("/Me/Tags/{tagId}", async (int tagId, GetUserNotesByTagHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(tagId, userId);
                
                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetUserNotesByTagEndpointName)
              .WithSummary("Get the current user's notes associated with a tag.")
              .RequireAuthorization()
              .Produces<List<NoteDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            notesGroup.MapPost("/", async (CreateNewNoteRequest request, CreateNewNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(request, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.CreatedAtRoute(GetNoteByIdEndpointName, new { noteId = result.Value.Id }, result.Value);
            }).WithName(CreateNewNoteEndpointName)
              .WithSummary("Create a note for the current user.")
              .RequireAuthorization()
              .Produces<NoteDto>(StatusCodes.Status201Created)
              .ProducesValidationProblem()
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound);

            notesGroup.MapPut("/{noteId}", async (int noteId, UpdateUserNoteRequest request, UpdateUserNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(request, userId, noteId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(UpdateUserNoteEndpointName)
              .WithSummary("Update a note owned by the current user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .ProducesValidationProblem()
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            notesGroup.MapDelete("/{noteId}", async (int noteId, DeleteUserNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(noteId, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(DeleteUserNoteEndpointName)
              .WithSummary("Delete a note owned by the current user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            notesGroup.MapPost("/{noteId}/Tags/{tagId}", async ( int noteId, int tagId, AttachTagToNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(noteId, tagId, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(AttachTagToNoteEndpointName)
              .WithSummary("Attach a tag to a note owned by the current user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden, ErrorType.Conflict);

            notesGroup.MapDelete("/{noteId}/Tags/{tagId}", async (int noteId, int tagId, DetachTagFromNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.GetUserId();

                var result = await handler.Handle(noteId, tagId, userId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(DetachTagFromNoteEndpointName)
              .WithSummary("Remove a tag from a note owned by the current user.")
              .RequireAuthorization()
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status401Unauthorized)
              .Produces<UnhandledExceptionResponse>(StatusCodes.Status500InternalServerError)
              .ProducesResultErrors(ErrorType.NotFound, ErrorType.Forbidden);

            return notesGroup;
        }
    }
}
