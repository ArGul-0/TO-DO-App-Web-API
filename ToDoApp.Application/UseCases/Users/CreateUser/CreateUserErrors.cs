using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    public class CreateUserErrors
    {
        public static readonly Error UserAlreadyExists = new Error(
            "UserAlreadyExists",
            "A user with the same username or email already exists.",
            ErrorType.Conflict);
    }
}
