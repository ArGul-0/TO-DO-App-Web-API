using System.ComponentModel.DataAnnotations;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.UseCases.Users.ChangeUserVisibility
{
    public record ChangeUserVisibilityRequest(
        [Required][Range(0, 1)] AccountVisibility newVisibility
        );
}
