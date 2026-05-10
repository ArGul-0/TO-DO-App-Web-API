using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;

namespace ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes
{
    public class GetAllOtherPeopleNotesHandler
    {
        public Task<ResultT<NoteDto>> Handle()
        {
            return Task.FromResult(
                ResultT<NoteDto>.Success(
                    new NoteDto(1, "Placeholder", "Placeholder")));
        }
    }
}
