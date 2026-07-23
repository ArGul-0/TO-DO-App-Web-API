using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.AcceptFriendRequest
{
    public class AcceptFriendshipRequestHandler
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AcceptFriendshipRequestHandler> logger;
        public AcceptFriendshipRequestHandler(
            IFriendshipRepository friendshipRepository,
            IUnitOfWork unitOfWork,
            ILogger<AcceptFriendshipRequestHandler> logger)
        {
            this.friendshipRepository = friendshipRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
            var friendship = await friendshipRepository.GetFriendshipAsync(userId, friendId);

            if(friendship is null)
                return Result.Failure(FriendshipErrors.FriendshipNotExists);

            if (friendship.AddresseeId != userId)
                return Result.Failure(FriendshipErrors.NotAllowedToManageThisFriendsipRequest);

            friendship.Accept();

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} accepted friend request from {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
