using FluentAssertions;
using Moq;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Application.UseCases.Users.GetUserById;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Users
{
    public sealed class GetUserByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();

            var user = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var userDto = user.ToDto();

            var handler = new GetUserByIdHandler(userRepository.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(userDto);

            userRepository.Verify(repo => repo.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var handler = new GetUserByIdHandler(userRepository.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            userRepository.Verify(repo => repo.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
