using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Notes
{
    public static class NotesErrors
    {
        public static readonly Error NotesNotFound = new Error("NotesNotFound",
            "No notes were found for the specified user.",
            ErrorType.NotFound);

        public static readonly Error NoteNotFound = new Error("NoteNotFound",
            "The specified note was not found.",
            ErrorType.NotFound);
    }
}
