using System.Security.Claims;
using ToDoApp.Application.UseCases.Notes.CreateNewNote;
using ToDoApp.Application.UseCases.Notes.DeleteUserNote;
using ToDoApp.Application.UseCases.Notes.GetAllNotes;
using ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes;
using ToDoApp.Application.UseCases.Notes.GetNoteById;
using ToDoApp.Application.UseCases.Notes.UpdateUserNote;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class NotesEndpoints
    {
        const string GetAllNotesEndpointName = "GetAllNotes"; // Constant For The GetAllNotes Endpoint Name
        const string GetNoteByIdEndpointName = "GetNoteById"; // Constant For The GetNoteById Endpoint Name
        const string GetAllUserNotesEndpointName = "GetAllUserNotes"; // Constant For The GetAllUserNotes Endpoint Name
        const string CreateNewNoteEndpointName = "CreateNewNote"; // Constant For The CreateNewNote Endpoint Name
        const string UpdateUserNoteEndpointName = "UpdateUserNote";
        const string DeleteUserNoteEndpointName = "DeleteUserNote"; // Constant For The DeleteUserNote Endpoint Name

        public static RouteGroupBuilder MapNotesEndpoints(this WebApplication app)
        {
            var notesGroup = app.MapGroup("/Notes"); // Create A Group For /Notes Endpoints

            notesGroup.MapGet("/", async (GetAllNotesHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllNotesEndpointName).RequireAuthorization();

            notesGroup.MapGet("/{id}", async (int id, GetNoteByIdHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(id, int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetNoteByIdEndpointName).RequireAuthorization();

            notesGroup.MapGet("/Me/", async (GetAllUserNotesHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllUserNotesEndpointName).RequireAuthorization();

            notesGroup.MapPost("/", async (CreateNewNoteRequest request, CreateNewNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(request, int.Parse(userId));

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.CreatedAtRoute(GetNoteByIdEndpointName, new { id = result.Value.Id }, result.Value);
            }).WithName(CreateNewNoteEndpointName).RequireAuthorization();

            notesGroup.MapPut("/{noteId}", async (int noteId, UpdateUserNoteRequest request, UpdateUserNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(request, int.Parse(userId), noteId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.NoContent();
            }).WithName(UpdateUserNoteEndpointName).RequireAuthorization();

            notesGroup.MapDelete("/{noteId}", async (int noteId, DeleteUserNoteHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId), noteId);

                if (result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok();
            }).WithName(DeleteUserNoteEndpointName).RequireAuthorization();

            return notesGroup;
        }
    }
}
