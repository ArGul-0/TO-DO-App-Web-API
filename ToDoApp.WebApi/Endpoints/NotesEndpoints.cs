using System.Security.Claims;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositoryes;
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

            notesGroup.MapPost("/CreateNewNote", async (HttpContext context, IUserRepository userRepository, IUnitOfWork unitOfWork) => // PLASEHOLDER FOR TESTS
            {
                int counter = 1;

                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var user = await userRepository.GetUserByIdAsync(int.Parse(userId));

                if (user is null)
                    throw new Exception("User Not Found");

                user.AddNote("Test Note", $"This Is A Test Note {counter}");

                await unitOfWork.SaveChangesAsync();

                counter++;
            }).RequireAuthorization();


            return notesGroup;
        }
    }
}
