using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Friends.SendFriendRequest;

namespace ToDoApp.Application.UseCases.Friends.AcceptFriendRequest
{
    internal class AcceptFriendRequestHandler
    {
        private readonly IFriendshipRepository friendRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AcceptFriendRequestHandler> logger;
        public AcceptFriendRequestHandler(
            IFriendshipRepository friendRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<AcceptFriendRequestHandler> logger)
        {
            this.friendRepository = friendRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int userId, int friendId)
        {
        }
    }
}
