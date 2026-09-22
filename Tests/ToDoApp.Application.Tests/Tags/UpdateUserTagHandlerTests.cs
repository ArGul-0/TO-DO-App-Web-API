using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Tags.UpdateUserTag;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Tags
{
    public sealed class UpdateUserTagHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<UpdateUserTagHandler>>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new UpdateUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            var request = new UpdateUserTagRequest(
                Name: "New Name"
            );

            // Act
            var result = await handler.Handle(request, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            tagRepository.Verify(r => r.GetTagByIdWithTrackingAsync(It.IsAny<int>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTagDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<UpdateUserTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            tagRepository.Setup(r => r.GetTagByIdWithTrackingAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)null);

            var handler = new UpdateUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            var request = new UpdateUserTagRequest(
                Name: "New Name"
            );

            // Act
            var result = await handler.Handle(request, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.TagNotFound);

            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<UpdateUserTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var tag = new Tag(
                "Test Tag",
                ownerId: 2
            );

            tagRepository.Setup(r => r.GetTagByIdWithTrackingAsync(It.IsAny<int>()))
                .ReturnsAsync(tag);

            var handler = new UpdateUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            var request = new UpdateUserTagRequest(
                Name: "New Name"
            );

            // Act
            var result = await handler.Handle(request, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.Forbidden);

            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTag_WhenUserOwnsTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<UpdateUserTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var tag = new Tag(
                "Test Tag",
                ownerId: 1
            );

            tagRepository.Setup(r => r.GetTagByIdWithTrackingAsync(It.IsAny<int>()))
                .ReturnsAsync(tag);

            var handler = new UpdateUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            var request = new UpdateUserTagRequest(
                Name: "New Name"
            );

            // Act
            var result = await handler.Handle(request, tagId: 1, userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            tag.Name.Should().Be("New Name");

            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
