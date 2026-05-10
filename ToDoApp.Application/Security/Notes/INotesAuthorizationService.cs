using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Security.Notes
{
    public interface INotesAuthorizationService
    {
        public bool CanSeeNote(User currentUser, Note note);
    }
}
