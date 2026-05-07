using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Notes.GetAllNotes
{
    public class GetAllNotesHandler
    {
        public Task<ResultT<List<NoteDto>>> Handle()
        {
            // Implementation To Get All Notes
            // This Is Just A Placeholder And Should Be Replaced With Actual Logic To Retrieve Notes From The Database Or Any Data Source
            var notes = new List<NoteDto>
            {
                new NoteDto { Id = Guid.NewGuid(), Title = "Sample Note 1", Content = "This is a sample note." },
                new NoteDto { Id = Guid.NewGuid(), Title = "Sample Note 2", Content = "This is another sample note." }
            };
            return Task.FromResult(Result<List<NoteDto>>.Success(notes));
        }
    }
}
