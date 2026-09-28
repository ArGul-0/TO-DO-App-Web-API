using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Notes.GetUserNotesByTag
{
    public class GetUserNotesByTagHandler
    {
        private readonly INoteRepository noteRepository;
        private readonly IUserRepository userRepository;

        public GetUserNotesByTagHandler(INoteRepository noteRepository,
            IUserRepository userRepository)
        {
            this.noteRepository = noteRepository;
            this.userRepository = userRepository;
        }

        public async Task<ResultT<List<NoteDto>>> Handle(int tagId, int userId)
        {
            // Implement the logic to get user notes by tag here
            // For now, just return an empty list as a placeholder
            return ResultT<List<NoteDto>>.Success(new List<NoteDto>());
        }
    }
}
