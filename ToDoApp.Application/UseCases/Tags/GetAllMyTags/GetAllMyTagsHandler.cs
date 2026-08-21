using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;

namespace ToDoApp.Application.UseCases.Tags.GetAllMyTags
{
    public class GetAllMyTagsHandler
    {
        private readonly ITagRepository tagRepository;
        private readonly IUserRepository userRepository;
        private readonly ILogger<GetAllMyTagsHandler> logger;

        public GetAllMyTagsHandler(ITagRepository tagRepository,
            IUserRepository userRepository,
            ILogger<GetAllMyTagsHandler> logger) 
        {
            this.tagRepository = tagRepository;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<ResultT<List<TagDto>>> Handle(int userId)
        {
            var existingUser = await userRepository.GetUserByIdAsync(userId);

            if(existingUser is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database.", userId);

                return ResultT<List<TagDto>>.Failure(UsersErrors.UserNotFound);
            }

            var tags = await tagRepository.GetAllTagsByUserIdAsync(userId);

            var tagDtos = tags.Select(t => t.ToDto()).ToList();

            return ResultT<List<TagDto>>.Success(tagDtos);
        }
    }
}
