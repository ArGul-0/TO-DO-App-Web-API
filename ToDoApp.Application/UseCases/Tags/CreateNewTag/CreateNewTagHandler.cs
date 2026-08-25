using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Tags.CreateNewTag
{
    public class CreateNewTagHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ITagRepository tagRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateNewTagHandler> logger;

        public CreateNewTagHandler(IUserRepository userRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateNewTagHandler> logger)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<ResultT<TagDto>> Handle(CreateNewTagRequest request, int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database.", userId);

                return ResultT<TagDto>.Failure(UsersErrors.UserNotFound);
            }

            var newTag = existingUser.AddTag(request.Name);

            await tagRepository.AddTagAsync(newTag);

            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("User with ID {UserId} created a new tag with name {TagName}.", userId, newTag.Name);

            return ResultT<TagDto>.Success(newTag.ToDto());
        }
    }
}
