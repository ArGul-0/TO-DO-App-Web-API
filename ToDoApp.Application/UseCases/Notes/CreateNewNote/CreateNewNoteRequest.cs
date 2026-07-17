using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Notes.CreateNewNote
{
    public record CreateNewNoteRequest(
        [Required] string Title,
        [Required] string Content,
        [Required] bool IsDone = false
    );
}
