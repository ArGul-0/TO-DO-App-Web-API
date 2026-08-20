using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.DTOs
{
    public record TagDto(
        [Required] int Id,
        [Required] string Name
        );
}
