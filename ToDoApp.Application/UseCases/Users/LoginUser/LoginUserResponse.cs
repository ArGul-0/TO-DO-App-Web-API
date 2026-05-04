using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Application.UseCases.Users.LoginUser
{
    public record LoginUserResponse(
        [Required] string Token
        );
}
