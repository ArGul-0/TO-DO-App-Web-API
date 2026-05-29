using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Friends.SendFriendRequest
{
    public class SendFriendshipsRequestErrors
    {
        public static Error CannotFriendYourself => new Error("CannotFriendYourself", "You cannot send a friend request to yourself.", ErrorType.Conflict);
    }
}
