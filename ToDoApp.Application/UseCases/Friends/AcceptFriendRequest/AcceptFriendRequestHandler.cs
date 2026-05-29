using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.AcceptFriendRequest
{
    public class AcceptFriendRequestHandler
    {
        private readonly IFriendshipRepository friendRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AcceptFriendRequestHandler> logger;
        public AcceptFriendRequestHandler(
            IFriendshipRepository friendRepository,
            IUnitOfWork unitOfWork,
            ILogger<AcceptFriendRequestHandler> logger)
        {
            this.friendRepository = friendRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
            var friendship = await friendRepository.GetFriendshipAsync(userId, friendId);

            if(friendship is null)
                return Result.Failure(FriendsErrors.FriendshipNotExists);

            if (friendship.AddresseeId != userId)
                return Result.Failure(FriendsErrors.CannotAcceptThisFriendRequest);

            friendship.Accept();

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} accepted friend request from {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
