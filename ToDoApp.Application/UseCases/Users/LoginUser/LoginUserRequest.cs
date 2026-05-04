using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Users.LoginUser
{
    public record LoginUserRequest(
        [Required][EmailAddress] string Email,
        [Required][MinLength(6)] string Password
        );
}
