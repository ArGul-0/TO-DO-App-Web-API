using Microsoft.Extensions.Logging;
using ToDoApp.Application.Common;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.UseCases.Users.CreateUser
{
    public class CreateUserHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IPasswordHasher passwordHasher;
        private readonly ILogger<CreateUserHandler> logger;

        public CreateUserHandler(IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher,
            ILogger<CreateUserHandler> logger)
        {
            this.userRepository = userRepository;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.passwordHasher = passwordHasher;
            this.logger = logger;
        }



        public async Task<ResultT<CreateUserResponse>> Handle(CreateUserRequest request)
        {
            var existingUserByUsername = await userRepository.GetUserByUsernameAsync(request.Username);

            if (existingUserByUsername != null)
                return ResultT<CreateUserResponse>.Failure(CreateUserErrors.UserAlreadyExists);

            var existingUserByEmail = await userRepository.GetUserByEmailAsync(request.Email);

            if (existingUserByEmail != null)
                return ResultT<CreateUserResponse>.Failure(CreateUserErrors.UserAlreadyExists);

            var newUser = new User(
                request.Username,
                new Email(request.Email),
                await passwordHasher.HashPasswordAsync(request.Password));

            await userRepository.AddUserAsync(newUser);

            var jwtToken = jwtTokenGenerator.GenerateAccessToken(newUser);

            logger.LogInformation("User created successfully: {Username}", newUser.Username);

            return ResultT<CreateUserResponse>.Success(new CreateUserResponse(
                User: new UserDto(
                    Id: newUser.Id,
                    Username: newUser.Username,
                    Email: newUser.Email.Value
                ),
                Token: jwtToken
            ));
        }
    }
}
