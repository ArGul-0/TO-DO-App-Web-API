using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.Security.Notes
{
    public class NotesAuthorizationService : INotesAuthorizationService
    {
        public bool CanSeeNote(User currentUser, Note note)
        {
            if (note.UserId == currentUser.Id)
                return true;

            else if (note.User.Visibility == AccountVisibility.Public)
                return true;

            return false;
        }

        public bool IsUserOwnsNote(User currentUser, Note note)
        {
            if (note.UserId == currentUser.Id)
                return true;

            return false;
        }

        public List<Note> FilterVisibleNotes(User currentUser, List<Note> notes)
        {
            return notes
                .Where(note => CanSeeNote(currentUser, note))
                .ToList();
        }
    }
}
