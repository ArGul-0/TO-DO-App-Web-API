using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.UseCases.Users.LoginUser
{
    public class LoginUserHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IPasswordHasher passwordHasher;
        private readonly ILogger<LoginUserHandler> logger;

        public LoginUserHandler(IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher,
            ILogger<LoginUserHandler> logger)
        {
            this.userRepository = userRepository;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.passwordHasher = passwordHasher;
            this.logger = logger;
        }



        public async Task<ResultT<LoginUserResponse>> Handle(LoginUserRequest request)
        {
            var isUserExists = await userRepository.GetUserByEmailAsync(request.Email);

            if(isUserExists is null)
            {
                logger.LogWarning("Login attempt with non-existent email: {Email}", request.Email);

                return ResultT<LoginUserResponse>.Failure(LoginUserErrors.UserNotFound);
            }
               
            bool isPasswordValid = await passwordHasher.VerifyPasswordAsync(request.Password, isUserExists.HashedPassword);

            if(isPasswordValid)
            {
                var jwtToken = jwtTokenGenerator.GenerateAccessToken(isUserExists);

                logger.LogInformation("User logged in successfully with email: {Email}", request.Email);

                return ResultT<LoginUserResponse>.Success(new LoginUserResponse( Token: jwtToken ));
            }
            else
            {
                logger.LogWarning("Failed login attempt for email: {Email}", request.Email);

                return ResultT<LoginUserResponse>.Failure(LoginUserErrors.InvalidPassword);
            }
        }
    }
}
