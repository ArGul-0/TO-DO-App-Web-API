using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.SendFriendRequest
{
    public class SendFriendRequestHandler
    {
        private readonly IFriendRepository friendRepository;
        public SendFriendRequestHandler()
        {
            
        }

        public Task<Result> Handle(int userId, int friendId)
        {

        }
    }
}
