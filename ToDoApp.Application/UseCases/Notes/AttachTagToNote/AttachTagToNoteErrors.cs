using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Notes.AttachTagToNote
{
    public class AttachTagToNoteErrors
    {
        public static readonly Error TagAlreadyAttachedToNote = new Error("TagAlreadyAttachedToNote",
            "The tag is already attached to the note.",
            ErrorType.Conflict);
    }
}
