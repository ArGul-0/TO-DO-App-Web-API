using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Notes.DetachTagFromNote
{
    public class DetachTagFromNoteHandler
    {
        public DetachTagFromNoteHandler()
        {
            
        }

        public async Task<Result> Handle(int noteId, int tagId, int userId)
        {

            return Result.Success();
        }
    }
}
