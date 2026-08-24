using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Tags
{
    public class TagsErrors
    {
        public static readonly Error TagNotFound = new Error("TagNotFound",
            "The specified tag was not found.",
            ErrorType.NotFound);

        public static readonly Error Forbidden = new Error("Forbidden",
            "You don't have permission to access this tag.",
            ErrorType.Forbidden);
    }
}
