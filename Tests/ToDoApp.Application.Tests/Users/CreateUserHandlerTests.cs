using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Users
{
    public sealed class CreateUserHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateUser_WhenRequestIsValid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<CreateUserHandler>>();

            userRepository.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            userRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            passwordHasher.Setup(hasher => hasher.HashPasswordAsync(It.IsAny<string>()))
                .ReturnsAsync("Example of hashed password");

            jwtTokenGenerator.Setup(generator => generator.GenerateAccessToken(It.IsAny<User>()))
                .Returns("Example of Jwt Token");

            var request = new CreateUserRequest(
                Username: "John Doe",
                Email: "john@example.com",
                Password: "Example of password"
                );

            var newUserDto = new UserDto(
                Id: 0,
                Username: request.Username,
                Email: request.Email,
                Visibility: AccountVisibility.Public
                );

            var handler = new CreateUserHandler(
                userRepository.Object,
                jwtTokenGenerator.Object,
                passwordHasher.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.User.Should().BeEquivalentTo(newUserDto);
            result.Value.Token.Should().Be("Example of Jwt Token");

            userRepository.Verify(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
            userRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);

            passwordHasher.Verify(hasher => hasher.HashPasswordAsync(It.IsAny<string>()), Times.Once);

            jwtTokenGenerator.Verify(generator => generator.GenerateAccessToken(It.IsAny<User>()), Times.Once);

            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<CreateUserHandler>>();

            var user = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password"
            );

            userRepository
                .Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var request = new CreateUserRequest(
                Username: "John Doe",
                Email: "john@example.com",
                Password: "Example of password"
                );

            var handler = new CreateUserHandler(
                userRepository.Object,
                jwtTokenGenerator.Object,
                passwordHasher.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CreateUserErrors.UserAlreadyExists);

            userRepository.Verify(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
            userRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Never);

            passwordHasher.Verify(hasher => hasher.HashPasswordAsync(It.IsAny<string>()), Times.Never);

            jwtTokenGenerator.Verify(generator => generator.GenerateAccessToken(It.IsAny<User>()), Times.Never);

            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<CreateUserHandler>>();

            var user = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password"
            );

            userRepository
                .Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            userRepository
                .Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var request = new CreateUserRequest(
                Username: "John Doe",
                Email: "john@example.com",
                Password: "Example of password"
                );

            var handler = new CreateUserHandler(
                userRepository.Object,
                jwtTokenGenerator.Object,
                passwordHasher.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CreateUserErrors.UserAlreadyExists);

            userRepository.Verify(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
            userRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);

            passwordHasher.Verify(hasher => hasher.HashPasswordAsync(It.IsAny<string>()), Times.Never);

            jwtTokenGenerator.Verify(generator => generator.GenerateAccessToken(It.IsAny<User>()), Times.Never);

            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
