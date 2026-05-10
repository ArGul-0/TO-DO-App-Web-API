using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes
{
    public class GetAllOtherPeopleNotesHandler
    {
        private readonly INotesRepository notesRepository;
        public GetAllOtherPeopleNotesHandler(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }

        public Task<ResultT<NoteDto>> Handle()
        {
            return Task.FromResult(
                ResultT<NoteDto>.Success(
                    new NoteDto(1, "Placeholder", "Placeholder")));
        }
    }
}
