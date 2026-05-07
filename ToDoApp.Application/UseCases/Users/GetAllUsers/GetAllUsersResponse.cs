using System.ComponentModel.DataAnnotations;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Users.GetAllUsers
{
    public record GetAllUsersResponse(
        [Required] List<UserDto> Users
        );
}
