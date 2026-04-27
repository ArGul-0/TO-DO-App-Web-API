using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    internal record CreateUserResponse(
        [Required] int id,
        [Required] string username,
        [Required][EmailAddress] string email,
        [Required] string jwtToken
        );
}
