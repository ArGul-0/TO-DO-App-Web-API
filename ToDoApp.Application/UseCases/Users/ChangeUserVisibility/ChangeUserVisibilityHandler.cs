using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users.CreateUser;

namespace ToDoApp.Application.UseCases.Users.ChangeUserVisibility
{
    public class ChangeUserVisibilityHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateUserHandler> logger;

        public ChangeUserVisibilityHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateUserHandler> logger)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handler(ChangeUserVisibilityRequest request, int userId)
        {
            var user = await userRepository.GetUserByIdWithTrackingAsync(userId);

            if (user is null)
                return Result.Failure(UsersErrors.UserNotFound);

            user.ChangeAccountVisibility(request.newVisibility);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User successfully change visibility to {newVisibility}", request.newVisibility);

            return Result.Success();
        }
    }
}
