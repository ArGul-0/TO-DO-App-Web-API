using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Notes.DetachTagFromNote
{
    public class DetachTagFromNoteErrors
    {
        public static readonly Error TagNotAttachedToNote = new Error("TagNotAttachedToNote",
            "The tag is not attached to the note.",
            ErrorType.NotFound);
    }
}
