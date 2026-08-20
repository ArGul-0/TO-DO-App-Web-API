using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.DTOs
{
    public record TagDto(
        [Required] string Name
        );
}
