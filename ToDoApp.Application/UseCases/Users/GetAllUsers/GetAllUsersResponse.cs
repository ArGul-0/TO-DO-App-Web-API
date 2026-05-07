using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Users.GetAllUsers
{
    public record GetAllUsersResponse(
        List<UserDto> Users
        );
}
