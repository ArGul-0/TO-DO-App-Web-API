using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.GetNoteById
{
    public class GetNoteByIdHandler
    {
        private readonly INotesRepository notesRepository;
        private readonly IUserRepository userRepository;
        private readonly INotesAuthorizationService notesAuthorizationService;

        public GetNoteByIdHandler(INotesRepository notesRepository, IUserRepository userRepository, INotesAuthorizationService notesAuthorizationService)
        {
            this.notesRepository = notesRepository;
            this.userRepository = userRepository;
            this.notesAuthorizationService = notesAuthorizationService;
        }

        public async Task<ResultT<NoteDto>> Handle(int id, int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user is null)
                return ResultT<NoteDto>.Failure(UsersErrors.UserNotFound);

            var note = await notesRepository.GetNoteByIdAsync(id);

            if (note is null)
                return ResultT<NoteDto>.Failure(NotesErrors.NoteNotFound);

            if (!notesAuthorizationService.CanSeeNote(user, note))
                return ResultT<NoteDto>.Failure(NotesErrors.Forbidden);

            return ResultT<NoteDto>.Success(note.ToDto());
        }
    }
}
