using ToDoApp.Application.DTOs;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Mappings
{
    public static class UserMappings
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto(
                Id: user.Id,
                Username: user.Username,
                Email: user.Email.Value,
                Visibility: user.Visibility
                );
        }
    }
}
