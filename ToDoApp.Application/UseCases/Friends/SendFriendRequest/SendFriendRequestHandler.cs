using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
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
            var newFriendship = new Friendship(userId, friendId);

            await friendRepository.AddFriendAsync(newFriendship);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User {UserId} sent a friend request to User {FriendId}", userId, friendId);

            return Result.Success();
        }
    }
}
