using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    internal class CreateUserHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IPasswordHasher passwordHasher;

        public CreateUserHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.passwordHasher = passwordHasher;
        }



        public async Task<ResultT<CreateUserResponse>> Handle(CreateUserRequest request)
        {
        }
    }
}
