using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Application.UseCases.Users.ChangeUserVisibility;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Users
{
    public sealed class ChangeUserVisibilityHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldChangeUserVisibility_WhenUserExists()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<ChangeUserVisibilityHandler>>();

            var user = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            userRepository.Setup(repo => repo.GetUserByIdWithTrackingAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var request = new ChangeUserVisibilityRequest(
                newVisibility: AccountVisibility.Private
                );

            var handler = new ChangeUserVisibilityHandler(
                userRepository.Object,
                unitOfWork.Object,
                logger.Object
                );

            // Act
            var result = await handler.Handle(request, 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            user.Visibility.Should().Be(AccountVisibility.Private);

            userRepository.Verify(repo => repo.GetUserByIdWithTrackingAsync(1), Times.Once);
            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<ChangeUserVisibilityHandler>>();

            userRepository.Setup(repo => repo.GetUserByIdWithTrackingAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var request = new ChangeUserVisibilityRequest(
                newVisibility: AccountVisibility.Private
                );

            var handler = new ChangeUserVisibilityHandler(
                userRepository.Object,
                unitOfWork.Object,
                logger.Object
                );

            // Act
            var result = await handler.Handle(request, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            userRepository.Verify(repo => repo.GetUserByIdWithTrackingAsync(1), Times.Once);
            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
