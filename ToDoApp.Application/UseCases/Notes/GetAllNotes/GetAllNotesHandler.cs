using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Notes.GetAllNotes
{
    public class GetAllNotesHandler
    {
        private readonly INotesRepository notesRepository;

        public GetAllNotesHandler(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }

        public async Task<ResultT<List<NoteDto>>> Handle(int userId)
        {
            var notes = await notesRepository.GetAllNotesAsync(userId);

            if(notes is null || !notes.Any())
                return ResultT<List<NoteDto>>.Failure(NotesErrors.NotesNotFound);

            var noteDtos = notes.Select(note => new NoteDto
            (
                Id:note.Id,
                Title: note.Title,
                Content: note.Content
            )).ToList();

            return ResultT<List<NoteDto>>.Success(noteDtos);
        }
    }
}
