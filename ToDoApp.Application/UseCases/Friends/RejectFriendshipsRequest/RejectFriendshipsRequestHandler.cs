using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.RejectFriendRequest
{
    public class RejectFriendshipsRequestHandler
    {
        private readonly IFriendshipRepository friendRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RejectFriendshipsRequestHandler> logger;
        public RejectFriendshipsRequestHandler(
            IFriendshipRepository friendRepository,
            IUnitOfWork unitOfWork,
            ILogger<RejectFriendshipsRequestHandler> logger)
        {
            this.friendRepository = friendRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
            var friendship = await friendRepository.GetFriendshipAsync(userId, friendId);

            if (friendship is null)
                return Result.Failure(FriendshipErrors.FriendshipNotExists);

            if (friendship.AddresseeId != userId)
                return Result.Failure(FriendshipErrors.NotAllowedToManageThisFriendRequest);

            friendship.Reject();

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} rejected friend request from {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
