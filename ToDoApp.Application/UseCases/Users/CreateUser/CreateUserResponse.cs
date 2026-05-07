using System.ComponentModel.DataAnnotations;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    public record CreateUserResponse(
        [Required] UserDto User,
        [Required] string JwtToken
        );
}
