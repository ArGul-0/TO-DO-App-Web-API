using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Notes.GetAllNotes
{
    public class GetAllNotesHandler
    {
        private readonly INotesRepository notesRepository;
        private readonly IUserRepository userRepository;
        private readonly INotesAuthorizationService notesAuthorizationService;

        public GetAllNotesHandler(INotesRepository notesRepository, IUserRepository userRepository, INotesAuthorizationService notesAuthorizationService)
        {
            this.notesRepository = notesRepository;
            this.userRepository = userRepository;
            this.notesAuthorizationService = notesAuthorizationService;
        }

        public async Task<ResultT<List<NoteDto>>> Handle(int userId)
        {
            var notes = await notesRepository.GetAllNotesWithOwnersAsync();

            if (notes is null || !notes.Any())
                return ResultT<List<NoteDto>>.Failure(NotesErrors.NotesNotFound);

            var user = await userRepository.GetUserByIdAsync(userId);

            if (user is null)
                return ResultT<List<NoteDto>>.Failure(UsersErrors.UserNotFound);

            var filteredNotes = notesAuthorizationService.FilterVisibleNotes(user, notes);

            var noteDtos = filteredNotes.Select(filteredNotes => filteredNotes.ToDto()).ToList();

            return ResultT<List<NoteDto>>.Success(noteDtos);
        }
    }
}
