using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Notes.UpdateUserNote
{
    public record UpdateUserNoteRequest(
        [Required] string Title,
        [Required] string Content,
        [Required] bool IsDone = false
        );
}
