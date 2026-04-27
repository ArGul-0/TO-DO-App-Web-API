using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    public record CreateUserResponse(
        [Required] int id,
        [Required] string username,
        [Required][EmailAddress] string email,
        [Required] string jwtToken
        );
}
