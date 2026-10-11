using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Security.Notes
{
    public interface INotesAuthorizationService
    {
        public Task<bool> CanSeeNote(User currentUser, Note note);
        public bool IsUserOwnsNote(int userId, Note note);
        public Task<List<Note>> FilterVisibleNotes(User currentUser, List<Note> notes);
    }
}
