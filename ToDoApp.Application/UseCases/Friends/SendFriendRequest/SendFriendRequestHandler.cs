using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.UseCases.Friends.SendFriendRequest
{
    public class SendFriendRequestHandler
    {
        private readonly IFriendRepository friendRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<SendFriendRequestHandler> logger;
        public SendFriendRequestHandler(
            IFriendRepository friendRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<SendFriendRequestHandler> logger)
        {
            this.friendRepository = friendRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
            if (userId == friendId)
            {
                return Result.Failure(SendFriendRequestErrors.CannotFriendYourself);
            }

            var user = await userRepository.GetUserByIdAsync(userId);
            var friend = await userRepository.GetUserByIdAsync(friendId);
            
            if (user is null)
                return Result.Failure(UsersErrors.UserNotFound);
            if (friend is null)
                return Result.Failure(FriendsErrors.FriendNotFound);

            // Normalization, so we always store the smaller userId as RequesterId and the larger one as AddresseeId
            userId = Math.Min(userId, friendId);
            friendId = Math.Max(userId, friendId);

            var newFriendship = new Friendship(userId, friendId);

            await friendRepository.AddFriendshipAsync(newFriendship);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} sent a friend request to User {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
