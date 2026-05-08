using System.Security.Claims;
using ToDoApp.Application.UseCases.Notes.GetAllNotes;
using ToDoApp.Application.UseCases.Notes.GetNoteById;
using ToDoApp.WebApi.Extensions;

namespace ToDoApp.WebApi.Endpoints
{
    public static class NotesEndpoints
    {
        const string GetAllNotesEndpointName = "GetAllNotes"; // Constant For The GetAllNotes Endpoint Name
        const string GetNoteByIdEndpointName = "GetNoteById"; // Constant For The GetNoteById Endpoint Name

        public static RouteGroupBuilder MapNotesEndpoints(this WebApplication app)
        {
            var notesGroup = app.MapGroup("/Notes"); // Create A Group For /Notes Endpoints

            notesGroup.MapGet("/", async (GetAllNotesHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(int.Parse(userId));

                if(result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetAllNotesEndpointName).RequireAuthorization();

            notesGroup.MapGet("/{id}", async (int id, GetNoteByIdHandler handler, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var result = await handler.Handle(id, int.Parse(userId));

                if(result.IsFailure)
                    return result.ToHttpResult();

                return Results.Ok(result.Value);
            }).WithName(GetNoteByIdEndpointName).RequireAuthorization();

            return notesGroup;
        }
    }
}
