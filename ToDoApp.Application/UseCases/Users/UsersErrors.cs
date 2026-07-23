using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Users
{
    public class UsersErrors
    {
        public static readonly Error UserNotFound = new Error(
            "UserNotFound",
            "The specified user was not found.",
            ErrorType.NotFound);
    }
}
