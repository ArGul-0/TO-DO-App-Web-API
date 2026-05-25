using ToDoApp.Application.Common;

namespace ToDoApp.Application.UseCases.Friends.SendFriendRequest
{
    public class SendFriendRequestHandler
    {
        private readonly IFriendRepository _friendRepository;
        public SendFriendRequestHandler()
        {
            
        }

        public Task<Result> Handle(int userId, int friendId)
        {

        }
    }
}
