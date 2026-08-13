using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Users.GetCurrentUser
{
    public class GetCurrentUserHandler
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<GetCurrentUserHandler> logger;
        public GetCurrentUserHandler(IUserRepository userRepository, ILogger<GetCurrentUserHandler> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<ResultT<UserDto>> Handle(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if(user is null)
            {
                logger.LogWarning("Authenticated user with ID {UserId} was not found in the database.", userId);

                return ResultT<UserDto>.Failure(UsersErrors.UserNotFound);
            }

            var userDto = user.ToDto();

            return ResultT<UserDto>.Success(userDto);
        }
    }
}
