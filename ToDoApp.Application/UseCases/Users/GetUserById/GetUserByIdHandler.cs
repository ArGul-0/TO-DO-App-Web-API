using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Users.GetUserById
{
    public class GetUserByIdHandler
    {
        private readonly IUserRepository userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<ResultT<UserDto>> Handle(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (user is null)
                return ResultT<UserDto>.Failure(UsersErrors.UserNotFound);

            var userDto = user.ToDto();

            return ResultT<UserDto>.Success(userDto);
        }
    }
}
