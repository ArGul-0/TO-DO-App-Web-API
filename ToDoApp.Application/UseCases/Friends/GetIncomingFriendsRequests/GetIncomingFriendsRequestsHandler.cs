using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests
{
    public class GetIncomingFriendsRequestsHandler
    {
        private readonly IFriendshipRepository friendshipRepository;
        public GetIncomingFriendsRequestsHandler(IFriendshipRepository friendshipRepository)
        {
            this.friendshipRepository = friendshipRepository;
        }

        public async Task<ResultT<List<FriendDto>>> Handle(int userId)
        {
            var friendships = await friendshipRepository.GetIncomingFriendshipRequestsAsync(userId);

            return ResultT<List<FriendDto>>.Success(friendships.Select(f => f.ToDto()).ToList());
        }
    }
}
