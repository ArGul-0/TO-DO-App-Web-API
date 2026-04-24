using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateAccessToken(User user);

    }
}
