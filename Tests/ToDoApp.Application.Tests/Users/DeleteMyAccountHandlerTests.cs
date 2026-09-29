using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Application.UseCases.Users.DeleteMyAccount;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Users
{
    public sealed class DeleteMyAccountHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var friendshipRepository = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteMyAccountHandler>>();

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new DeleteMyAccountHandler(
                userRepository.Object,
                friendshipRepository.Object,
                unitOfWork.Object,
                logger.Object
                );

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            userRepository.Verify(repo => repo.GetUserByIdAsync(1), Times.Once);

            friendshipRepository.Verify(
                repo => repo.GetAllFriendshipsByUserIdForDeletionAsync(It.IsAny<int>()),
                Times.Never
                );

            friendshipRepository.Verify(
                repo => repo.DeleteFriendshipAsync(It.IsAny<Friendship>()),
                Times.Never
                );

            userRepository.Verify(
                repo => repo.DeleteUserAsync(It.IsAny<User>()),
                Times.Never
                );

            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenUserExistsWithoutFriendships()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var friendshipRepository = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteMyAccountHandler>>();

            var user = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password"
                );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            friendshipRepository
                .Setup(repo => repo.GetAllFriendshipsByUserIdForDeletionAsync(It.IsAny<int>()))
                .ReturnsAsync([]);

            var handler = new DeleteMyAccountHandler(
                userRepository.Object,
                friendshipRepository.Object,
                unitOfWork.Object,
                logger.Object
                );

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            userRepository.Verify(repo => repo.GetUserByIdAsync(1), Times.Once);

            friendshipRepository.Verify(
                repo => repo.GetAllFriendshipsByUserIdForDeletionAsync(1),
                Times.Once
                );

            friendshipRepository.Verify(
                repo => repo.DeleteFriendshipAsync(It.IsAny<Friendship>()),
                Times.Never
                );

            userRepository.Verify(repo => repo.DeleteUserAsync(user), Times.Once);

            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldDeleteUserAndAllFriendships_WhenUserHasFriendships()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var friendshipRepository = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteMyAccountHandler>>();

            var user = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password"
                );

            var friendshipOne = new Friendship(
                requesterId: 1,
                addresseeId: 2
                );

            var friendshipTwo = new Friendship(
                requesterId: 3,
                addresseeId: 1
                );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            friendshipRepository
                .Setup(repo => repo.GetAllFriendshipsByUserIdForDeletionAsync(It.IsAny<int>()))
                .ReturnsAsync(
                [
                    friendshipOne,
                    friendshipTwo
                ]);

            var handler = new DeleteMyAccountHandler(
                userRepository.Object,
                friendshipRepository.Object,
                unitOfWork.Object,
                logger.Object
                );

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            userRepository.Verify(repo => repo.GetUserByIdAsync(1), Times.Once);

            friendshipRepository.Verify(
                repo => repo.GetAllFriendshipsByUserIdForDeletionAsync(1),
                Times.Once
                );

            friendshipRepository.Verify(
                repo => repo.DeleteFriendshipAsync(friendshipOne),
                Times.Once
                );

            friendshipRepository.Verify(
                repo => repo.DeleteFriendshipAsync(friendshipTwo),
                Times.Once
                );

            userRepository.Verify(repo => repo.DeleteUserAsync(user), Times.Once);

            unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}