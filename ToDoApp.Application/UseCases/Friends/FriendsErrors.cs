using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Friends
{
    public class FriendsErrors
    {
        public static Error FriendNotFound => new Error("FriendNotFound", "The specified friend was not found.", ErrorType.NotFound);
        public static Error FriendshipAlreadyExists => new Error("FriendshipAlreadyExists", "A friendship already exists between these users.", ErrorType.Conflict);
    }
}
