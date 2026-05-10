using System.ComponentModel.DataAnnotations;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.DTOs
{
    public record UserDto(
        [Required] int Id,
        [Required] string Username,
        [Required][EmailAddress] string Email,
        [Required] AccountVisibility Visibility
        );
}
