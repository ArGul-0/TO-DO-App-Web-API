using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.Security.Notes
{
    public class NotesAuthorizationService : INotesAuthorizationService
    {
        private readonly IFriendshipRepository friendshipRepository;

        public NotesAuthorizationService(IFriendshipRepository friendshipRepository)
        {
            this.friendshipRepository = friendshipRepository;
        }

        public async Task<bool> CanSeeNote(User currentUser, Note note)
        {
            if (note.UserId == currentUser.Id)
                return true;

            else if (note.User.Visibility == AccountVisibility.Public)
                return true;

            else if (note.User.Visibility == AccountVisibility.OnlyForFriends)
            {
                var friendship = await friendshipRepository.GetFriendshipAsync(currentUser.Id, note.UserId);

                if (friendship != null && friendship.Status == FriendshipStatus.Accepted)
                    return true;
            }

            return false;
        }

        public bool IsUserOwnsNote(int userId, Note note)
        {
            if (note.UserId == userId)
                return true;

            return false;
        }

        public async Task<List<Note>> FilterVisibleNotes(User currentUser, List<Note> notes)
        {
            var visibleNotes = new List<Note>();

            foreach (var note in notes)
            {
                if (await CanSeeNote(currentUser, note))
                {
                    visibleNotes.Add(note);
                }
            }

            return visibleNotes;
        }
    }
}
