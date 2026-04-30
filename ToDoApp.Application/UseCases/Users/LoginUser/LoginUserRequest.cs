using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Users.LoginUser
{
    internal record LoginUserRequest(
        [Required][EmailAddress] string Email,
        [Required][MinLength(6)] string Password
        );
}
