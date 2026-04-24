namespace ToDoApp.Application.Common
{
    public enum ErrorType
    {
        None,
        Validation,
        NotFound,
        Unauthorized,
        Forbidden,
        Conflict,
        InternalServerError
    }
}
