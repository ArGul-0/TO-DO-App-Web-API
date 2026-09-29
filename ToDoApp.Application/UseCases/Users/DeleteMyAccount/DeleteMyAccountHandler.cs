using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Users.DeleteMyAccount
{
    public class DeleteMyAccountHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IFriendshipRepository friendshipRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<DeleteMyAccountHandler> logger;

        public DeleteMyAccountHandler(IUserRepository userRepository,
            IFriendshipRepository friendshipRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteMyAccountHandler> logger)
        {
            this.userRepository = userRepository;
            this.friendshipRepository = friendshipRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database while deleting their account.", userId);

                return Result.Failure(UsersErrors.UserNotFound);
            }

            // Delete all friendships for the user
            var friendships = await friendshipRepository.GetAllFriendshipsByUserIdAsync(userId);
            foreach (var friendship in friendships)
            {
                await friendshipRepository.DeleteFriendshipAsync(friendship);
            }

            // Delete the user
            await userRepository.DeleteUserAsync(existingUser);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with ID {UserId} has successfully deleted their account.", userId);

            return Result.Success();
        }

    }
}
