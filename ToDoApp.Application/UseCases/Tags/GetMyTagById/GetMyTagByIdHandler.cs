using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Tags.GetMyTagById
{
    public class GetMyTagByIdHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILogger<GetMyTagByIdHandler> logger;

        public GetMyTagByIdHandler(IUserRepository userRepository,
            ITagRepository tagRepository,
            ILogger<GetMyTagByIdHandler> logger)
        {
            this.userRepository = userRepository;
            this.tagRepository = tagRepository;
            this.logger = logger;
        }

        public async Task<ResultT<TagDto>> Handle(int tagId, int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database.", userId);

                return ResultT<TagDto>.Failure(UsersErrors.UserNotFound);
            }

            var tag = await tagRepository.GetTagByIdAsync(tagId);

            if (tag is null)
            {
                return ResultT<TagDto>.Failure(TagsErrors.TagNotFound);
            }

            if (tag.OwnerId != userId)
            {
                return ResultT<TagDto>.Failure(TagsErrors.Forbidden);
            }

            return ResultT<TagDto>.Success(tag.ToDto());
        }
    }
}
