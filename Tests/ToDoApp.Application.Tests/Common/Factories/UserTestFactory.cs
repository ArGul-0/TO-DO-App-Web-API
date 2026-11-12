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
            return Create(username, new Email(email), password, id);
        }

        public static User Create(string username, Email email, string password, int id = 0)
        {
            var user = new User(username, email, password);

            typeof(User)
                .GetProperty(nameof(User.Id))!
                .SetValue(user, id);

            return user;
        }
    }
}
