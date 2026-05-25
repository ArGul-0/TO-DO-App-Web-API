using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes
{
    public class GetAllUserNotesHandler
    {
        private readonly INoteRepository notesRepository;
        public GetAllUserNotesHandler(INoteRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }

        public async Task<ResultT<List<NoteDto>>> Handle(int userId)
        {
            var notes = await notesRepository.GetAllNotesByUserIdAsync(userId);

            if (notes is null || !notes.Any())
                return ResultT<List<NoteDto>>.Failure(NotesErrors.NotesNotFound);

            var noteDtos = notes.Select(notes => notes.ToDto()).ToList();

            return ResultT<List<NoteDto>>.Success(noteDtos);
        }
    }
}
