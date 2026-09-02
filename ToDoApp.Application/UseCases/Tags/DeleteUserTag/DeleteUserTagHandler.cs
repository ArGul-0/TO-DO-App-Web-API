using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Tags.DeleteUserTag
{
    public class DeleteUserTagHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<DeleteUserTagHandler> logger;

        public DeleteUserTagHandler(IUserRepository userRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteUserTagHandler> logger)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(int tagId, int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database.", userId);

                return Result.Failure(UsersErrors.UserNotFound);
            }

            var existingTag = await tagRepository.GetTagByIdAsync(tagId);

            if (existingTag is null)
            {
                return Result.Failure(TagsErrors.TagNotFound);
            }

            if (existingTag.OwnerId != userId)
            {
                return Result.Failure(TagsErrors.Forbidden);
            }

            await tagRepository.DeleteTagAsync(existingTag);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with ID {UserId} deleted tag with ID {TagId}.", userId, tagId);

            return Result.Success();
        }
    }
}
