using System.ComponentModel.DataAnnotations;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Users.LoginUser
{
    public record LoginUserResponse(
        [Required] UserDto User,
        [Required] string Token
        );
}
