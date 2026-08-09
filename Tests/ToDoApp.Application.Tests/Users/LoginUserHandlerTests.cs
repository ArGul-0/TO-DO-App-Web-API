using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users.LoginUser;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Users
{
    public sealed class LoginUserHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldLoginUser_WhenRequestIsValid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var logger = new Mock<ILogger<LoginUserHandler>>();

            var existedUser = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password"
                );

            userRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existedUser);

            passwordHasher.Setup(hasher => hasher.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenGenerator.Setup(generator => generator.GenerateAccessToken(It.IsAny<User>()))
                .Returns("Example of Jwt Token");

            var request = new LoginUserRequest(
                Email: "john@example.com",
                Password: "Example of password"
                );

            var handler = new LoginUserHandler(
                userRepository.Object,
                jwtTokenGenerator.Object,
                passwordHasher.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Token.Should().Be("Example of Jwt Token");
            result.Value.User.Should().BeEquivalentTo(existedUser.ToDto());

            userRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);

            passwordHasher.Verify(hasher => hasher.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            jwtTokenGenerator.Verify(generator => generator.GenerateAccessToken(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var logger = new Mock<ILogger<LoginUserHandler>>();

            userRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var request = new LoginUserRequest(
                Email: "john@example.com",
                Password: "Example of password"
                );

            var handler = new LoginUserHandler(
                userRepository.Object,
                jwtTokenGenerator.Object,
                passwordHasher.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(LoginUserErrors.UserNotFound);

            userRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);

            passwordHasher.Verify(hasher => hasher.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            jwtTokenGenerator.Verify(generator => generator.GenerateAccessToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPasswordIsNotValid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var logger = new Mock<ILogger<LoginUserHandler>>();

            var existedUser = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password"
                );

            userRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existedUser);

            passwordHasher.Setup(hasher => hasher.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var request = new LoginUserRequest(
                Email: "john@example.com",
                Password: "Example of password"
                );

            var handler = new LoginUserHandler(
                userRepository.Object,
                jwtTokenGenerator.Object,
                passwordHasher.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(LoginUserErrors.InvalidPassword);

            userRepository.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);

            passwordHasher.Verify(hasher => hasher.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            jwtTokenGenerator.Verify(generator => generator.GenerateAccessToken(It.IsAny<User>()), Times.Never);
        }
    }
}
