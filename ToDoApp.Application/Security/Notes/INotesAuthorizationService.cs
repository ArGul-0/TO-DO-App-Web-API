using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Security.Notes
{
    public interface INotesAuthorizationService
    {
        public bool CanSeeNote(User currentUser, Note note);
        public bool IsUserOwnsNote(User currentUser, Note note);
        List<Note> FilterVisibleNotes(User currentUser, List<Note> notes);
    }
}
