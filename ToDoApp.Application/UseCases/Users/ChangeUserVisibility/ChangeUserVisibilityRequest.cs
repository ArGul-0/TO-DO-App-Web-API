using System.ComponentModel.DataAnnotations;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.UseCases.Users.ChangeUserVisibility
{
    public record ChangeUserVisibilityRequest(
        [Required] AccountVisibility newVisibility
        );
}
