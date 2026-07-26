using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Friends;
using ToDoApp.Application.UseCases.Friends.RemoveFriendship;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests
{
    public sealed class RemoveFriendshipHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFriendshipDoesNotExist()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RemoveFriendshipHandler>>();

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Friendship?)null);

            var handler = new RemoveFriendshipHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.FriendshipNotExists);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFriendshipIsNotAccepted()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RemoveFriendshipHandler>>();

            var friendship = new Friendship(1, 2);

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(friendship);

            var handler = new RemoveFriendshipHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.FriendshipIsNotAccepted);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserIsNotParticipant()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RemoveFriendshipHandler>>();

            var friendship = new Friendship(1, 2);
            friendship.Accept();

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(friendship);

            var handler = new RemoveFriendshipHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(3, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.NotAllowedToManageThisFriendsipRequest);
        }

        [Fact]
        public async Task Handle_ShouldRemoveFriendship_WhenRequestIsValid()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<RemoveFriendshipHandler>>();

            var friendship = new Friendship(1, 2);
            friendship.Accept();

            var addressee = new User("John Doe", new Email("a@b.com"), "Example of hashed password");
            var requester = new User("Jane Smith", new Email("b@c.com"), "Example of hashed password");

            typeof(Friendship).GetProperty("Addressee", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(friendship, addressee);
            typeof(Friendship).GetProperty("Requester", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(friendship, requester);

            friendshipRepo.Setup(r => r.GetFriendshipAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(friendship);

            var handler = new RemoveFriendshipHandler(
                friendshipRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(2, 1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            friendshipRepo.Verify(r => r.DeleteFriendship(friendship), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
