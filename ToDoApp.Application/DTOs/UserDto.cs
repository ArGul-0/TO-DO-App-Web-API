using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.DTOs
{
    public record UserDto(
        [Required] int Id,
        [Required] string Username,
        [Required][EmailAddress] string Email
        );
}
