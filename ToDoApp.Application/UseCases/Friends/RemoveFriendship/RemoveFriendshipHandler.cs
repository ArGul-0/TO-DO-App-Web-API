using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.UseCases.Friends.RemoveFriendship
{
    public class RemoveFriendshipHandler
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RemoveFriendshipHandler> logger;
        public RemoveFriendshipHandler(
            IFriendshipRepository friendshipRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveFriendshipHandler> logger)
        {
            this.friendshipRepository = friendshipRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
            var friendship = await friendshipRepository.GetFriendshipAsync(userId, friendId);

            if (friendship is null)
                return Result.Failure(FriendshipErrors.FriendshipNotExists);

            if (friendship.Status != FriendshipStatus.Accepted)
                return Result.Failure(FriendshipErrors.FriendshipIsNotAccepted);

            if (friendship.AddresseeId != userId && friendship.RequesterId != userId)
                return Result.Failure(FriendshipErrors.NotAllowedToManageThisFriendsipRequest);

            friendshipRepository.DeleteFriendship(friendship);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} removed friend {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
