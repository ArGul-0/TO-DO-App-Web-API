using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;

namespace ToDoApp.Application.UseCases.Users.GetAllUsers
{
    public class GetAllUsersHandler
    {
        private readonly IUserRepository userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<ResultT<GetAllUsersResponse>> Handle()
        {
            var users = await userRepository.GetAllUsersAsync();

            var userDtos = users
                .Select(u => new UserDto(u.Id, u.Username, u.Email.Value))
                .ToList();

            return ResultT<GetAllUsersResponse>.Success(new GetAllUsersResponse(userDtos));
        }
    }
}
