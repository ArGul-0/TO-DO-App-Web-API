using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Notes.GetUserNotesByTag
{
    public class GetUserNotesByTagHandler
    {
        public GetUserNotesByTagHandler()
        {
            
        }

        public async Task<ResultT<List<NoteDto>>> Handle(int tagId, int userId)
        {
            // Implement the logic to get user notes by tag here
            // For now, just return an empty list as a placeholder
            return ResultT<List<NoteDto>>.Success(new List<NoteDto>());
        }
    }
}
