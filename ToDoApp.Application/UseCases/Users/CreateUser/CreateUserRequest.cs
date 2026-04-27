using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    public record CreateUserRequest(
        [Required]string Username,
        [Required][EmailAddress]string Email,
        [Required][MinLength(6)]string Password
    );
}
