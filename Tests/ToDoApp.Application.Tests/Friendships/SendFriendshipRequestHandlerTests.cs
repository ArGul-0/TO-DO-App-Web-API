using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Friends;
using ToDoApp.Application.UseCases.Friends.SendFriendRequest;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests
{
    public sealed class SendFriendshipRequestHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserTriesToFriendHimself()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var userRepo = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<SendFriendshipRequestHandler>>();

            var handler = new SendFriendshipRequestHandler(
                friendshipRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(SendFriendshipRequestErrors.CannotFriendYourself);

            friendshipRepo.Verify(r => r.AddFriendshipAsync(It.IsAny<Friendship>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var userRepo = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<SendFriendshipRequestHandler>>();

            userRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new SendFriendshipRequestHandler(
                friendshipRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            friendshipRepo.Verify(r => r.AddFriendshipAsync(It.IsAny<Friendship>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFriendDoesNotExist()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var userRepo = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<SendFriendshipRequestHandler>>();

            userRepo.SetupSequence(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User("John Doe", new Email("john.doe@example.com"), "Example of hashed password"))
                .ReturnsAsync((User?)null);

            var handler = new SendFriendshipRequestHandler(
                friendshipRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.FriendNotFound);

            friendshipRepo.Verify(r => r.AddFriendshipAsync(It.IsAny<Friendship>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFriendshipAlreadyExists()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var userRepo = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<SendFriendshipRequestHandler>>();

            userRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User("John Doe", new Email("john.doe@example.com"), "Example of hashed password"));

            friendshipRepo.Setup(r => r.FriendshipExistsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var handler = new SendFriendshipRequestHandler(
                friendshipRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(FriendshipErrors.FriendshipAlreadyExists);

            friendshipRepo.Verify(r => r.AddFriendshipAsync(It.IsAny<Friendship>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateFriendship_WhenRequestIsValid()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();
            var userRepo = new Mock<IUserRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<SendFriendshipRequestHandler>>();

            userRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User("John Doe", new Email("john.doe@example.com"), "Example of hashed password"));

            friendshipRepo.Setup(r => r.FriendshipExistsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            var handler = new SendFriendshipRequestHandler(
                friendshipRepo.Object,
                userRepo.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 2);

            // Assert
            result.IsSuccess.Should().BeTrue();

            friendshipRepo.Verify(r => r.AddFriendshipAsync(It.Is<Friendship>(f => f.RequesterId == 1 && f.AddresseeId == 2)), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
