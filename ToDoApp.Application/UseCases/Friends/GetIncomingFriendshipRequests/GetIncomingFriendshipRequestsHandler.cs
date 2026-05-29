using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
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

        public async Task<ResultT<List<FriendshipDto>>> Handle(int userId, int friendId)
        {
            var friendships = await friendshipRepository.GetIncomingFriendshipsRequestsAsync(userId);

            if (friendships is null)
                return ResultT<List<FriendshipDto>>.Failure(FriendshipErrors.FriendshipNotExists);

            if (!friendships.Any(f => f.AddresseeId == userId))
                return ResultT<List<FriendshipDto>>.Failure(FriendshipErrors.NotAllowedToManageThisFriendsipRequest);

            return ResultT<List<FriendshipDto>>.Success(friendships.Select(f => f.ToDto()).ToList());
        }
    }
}
