using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.DTOs
{
    public record NoteDto(
        [Required] int Id,
        [Required] string Title,
        string Content
    );
}
