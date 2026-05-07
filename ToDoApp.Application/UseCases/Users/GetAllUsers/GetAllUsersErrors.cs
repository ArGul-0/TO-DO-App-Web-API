using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Users.GetAllUsers
{
    public static class GetAllUsersErrors
    {
        public static readonly Error NoUsersFound = new Error(
            "NoUsersFound",
            "No users were found in the system.",
            ErrorType.NotFound);
    }
}
