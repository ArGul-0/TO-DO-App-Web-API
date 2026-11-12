using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Common.Factories
{
    public static class UserTestFactory
    {
        public static User Create(
            string username = "John Doe",
            string email = "john.doe@example.com",
            string password = "Example of hashed password",
            int id = 1)
        {
            var user = new User(
                username,
                new Email(email),
                password);

            typeof(User)
                .GetProperty(nameof(User.Id))!
                .SetValue(user, id);

            return user;
        }
    }
}
