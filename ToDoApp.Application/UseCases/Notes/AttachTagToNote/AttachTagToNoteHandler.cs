using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Notes.AttachTagToNote
{
    public class AttachTagToNoteHandler
    {
        public AttachTagToNoteHandler()
        {
            
        }

        public async Task<Result> Handle(int noteId, int tagId, int userId)
        {

            return Result.Success();
        }
    }
}
