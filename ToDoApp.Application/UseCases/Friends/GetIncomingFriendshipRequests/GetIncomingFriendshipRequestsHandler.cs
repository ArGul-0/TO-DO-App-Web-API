using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests
{
    public class GetIncomingFriendshipRequestsHandler
    {
        private readonly IFriendshipRepository friendshipRepository;
        public GetIncomingFriendshipRequestsHandler(IFriendshipRepository friendshipRepository)
        {
            this.friendshipRepository = friendshipRepository;
        }

        public async Task<ResultT<List<FriendshipDto>>> Handle(int userId)
        {
            var friendships = await friendshipRepository.GetIncomingFriendshipsRequestsAsync(userId);

            return ResultT<List<FriendshipDto>>.Success(friendships.Select(f => f.ToDto()).ToList());
        }
    }
}
