using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.DTOs
{
    public record NoteDto(
        [Required] int Id,
        [Required] string Title,
        [Required] string Content,
        [Required] DateTime CreatedAt,
        [Required] DateTime UpdatedAt,
        [Required] bool IsDone = false
    );
}
