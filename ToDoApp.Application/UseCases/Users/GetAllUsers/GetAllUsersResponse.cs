namespace ToDoApp.Application.UseCases.Users.GetAllUsers
{
    public record GetAllUsersResponse(
        List<UserDto> Users
        );
}
