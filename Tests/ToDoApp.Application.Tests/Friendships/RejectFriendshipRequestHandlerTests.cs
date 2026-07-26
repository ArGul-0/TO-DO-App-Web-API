using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Friends;
using ToDoApp.Application.UseCases.Friends.RejectFriendRequest;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests
{
    public sealed class RejectFriendshipRequestHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFriendshipDoesNotExist()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RejectFriendshipRequestHandler>>();

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Friendship?)null);

            var handler = new RejectFriendshipRequestHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.FriendshipNotExists);

            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserIsNotAddressee()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RejectFriendshipRequestHandler>>();

            var friendship = new Friendship(1, 2);

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(friendship);

            var handler = new RejectFriendshipRequestHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.NotAllowedToManageThisFriendsipRequest);

            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldRejectFriendship_WhenUserIsAddressee()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RejectFriendshipRequestHandler>>();

            var friendship = new Friendship(1, 2);

            var addressee = new User("John Doe", new Email("john.doe@example.com"), "Example of hashed password");
            var requester = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");

            typeof(Friendship).GetProperty("Addressee", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(friendship, addressee);
            typeof(Friendship).GetProperty("Requester", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(friendship, requester);

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(friendship);

            var handler = new RejectFriendshipRequestHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(2, 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
            friendship.Status.Should().Be(Domain.Enums.FriendshipStatus.Rejected);
        }
    }
}
