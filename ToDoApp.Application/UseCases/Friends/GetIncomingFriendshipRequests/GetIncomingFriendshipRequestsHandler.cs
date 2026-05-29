using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
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

        public async Task<Result> Handle(int userId, int friendId)
        {
            var friendships = await friendshipRepository.GetIncomingFriendshipsRequestsAsync(userId);

            if (friendships is null)
                return Result.Failure(FriendshipErrors.FriendshipNotExists);

            if (!friendships.Any(f => f.AddresseeId == userId))
                return Result.Failure(FriendshipErrors.NotAllowedToManageThisFriendsipRequest);

            return Result.Success();
        }
    }
}
