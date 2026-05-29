using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.UseCases.Friends.SendFriendRequest
{
    public class SendFriendshipsRequestHandler
    {
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<SendFriendshipsRequestHandler> logger;
        public SendFriendshipsRequestHandler(
            IFriendshipRepository friendshipRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<SendFriendshipsRequestHandler> logger)
        {
            this.friendshipRepository = friendshipRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
            if (userId == friendId)
            {
                return Result.Failure(SendFriendshipsRequestErrors.CannotFriendYourself);
            }

            var user = await userRepository.GetUserByIdAsync(userId);
            var friend = await userRepository.GetUserByIdAsync(friendId);
            
            if (user is null)
                return Result.Failure(UsersErrors.UserNotFound);
            if (friend is null)
                return Result.Failure(FriendshipErrors.FriendNotFound);

            var existingFriendship = await friendshipRepository.FriendshipExistsAsync(userId, friendId);
            if (existingFriendship)
                return Result.Failure(FriendshipErrors.FriendshipAlreadyExists);

            var newFriendship = new Friendship(userId, friendId);

            await friendshipRepository.AddFriendshipAsync(newFriendship);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} sent a friend request to User {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
