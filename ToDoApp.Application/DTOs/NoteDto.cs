using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.DTOs
{
    public record NoteDto(
        [Required] int Id,
        [Required] string Title,
        [Required] string Content,
        [Required] bool IsDone,
        [Required] List<TagDto> Tags, 
        [Required] DateTime CreatedAt,
        [Required] DateTime UpdatedAt
    );
}
