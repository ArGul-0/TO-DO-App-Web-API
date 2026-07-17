using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.Interfaces.Repositories;

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
                .Select(u => u.ToDto())
                .ToList();

            return ResultT<GetAllUsersResponse>.Success(new GetAllUsersResponse(userDtos));
        }
    }
}
