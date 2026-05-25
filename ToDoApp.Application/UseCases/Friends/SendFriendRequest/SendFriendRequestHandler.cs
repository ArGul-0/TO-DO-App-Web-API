using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.SendFriendRequest
{
    public class SendFriendRequestHandler
    {
        private readonly IFriendRepository friendRepository;
        public SendFriendRequestHandler(IFriendRepository friendRepository)
        {
            this.friendRepository = friendRepository;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {

        }
    }
}
