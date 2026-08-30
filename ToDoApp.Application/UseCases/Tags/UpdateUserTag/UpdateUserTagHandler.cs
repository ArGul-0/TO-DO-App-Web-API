using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Tags.UpdateUserTag
{
    public class UpdateUserTagHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UpdateUserTagHandler> logger;

        public UpdateUserTagHandler(IUserRepository userRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateUserTagHandler> logger)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<Result> Handle(UpdateUserTagRequest request, int tagId, int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database.", userId);

                return Result.Failure(UsersErrors.UserNotFound);
            }

            var existingTag = await tagRepository.GetTagByIdWithTrackingAsync(tagId);

            if(existingTag is null)
            {
                return Result.Failure(TagsErrors.TagNotFound);
            }

            if(existingTag.OwnerId != userId)
            {
                return Result.Failure(TagsErrors.Forbidden);
            }

            existingTag.ChangeName(request.Name);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with ID {UserId} updated tag with ID {TagId}.", userId, tagId);

            return Result.Success();
        }
    }
}
