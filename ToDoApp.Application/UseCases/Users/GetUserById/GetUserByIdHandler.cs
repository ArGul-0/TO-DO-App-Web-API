using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;

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

            var userDto = new UserDto(user.Id, user.Username, user.Email.Value);

            return ResultT<UserDto>.Success(userDto);
        }
    }
}
