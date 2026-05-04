using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Users.LoginUser
{
    internal class LoginUserErrors
    {
        public static readonly Error UserNotFound = new("UserNotFound", "No user found with the provided email.", ErrorType.NotFound);
        public static readonly Error InvalidPassword = new("InvalidPassword", "The provided password is incorrect.", ErrorType.Unauthorized);
    }
}
