using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Friends
{
    public class FriendshipErrors
    {
        public static Error FriendNotFound => new Error("FriendNotFound", "The specified friend was not found.", ErrorType.NotFound);
        public static Error FriendshipAlreadyExists => new Error("FriendshipAlreadyExists", "A friendship already exists between these users.", ErrorType.Conflict);
        public static Error FriendshipNotExists => new Error("FriendshipNotExists", "No friendship exists between these users.", ErrorType.NotFound);
        public static Error NotAllowedToManageThisFriendRequest => new Error("NotAllowedToManageThisFriendRequest", "You are not allowed to manage this friend request.", ErrorType.Unauthorized);
        public static Error FriendshipIsNotAccepted => new Error("FriendshipIsNotAccepted", "The friendship is not accepted yet.", ErrorType.Conflict);
    }
}
