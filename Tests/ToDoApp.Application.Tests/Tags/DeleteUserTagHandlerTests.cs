using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Tags.DeleteUserTag;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Tags
{
    public sealed class DeleteUserTagHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteUserTagHandler>>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new DeleteUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            tagRepository.Verify(r => r.GetTagByIdAsync(It.IsAny<int>()), Times.Never);
            tagRepository.Verify(r => r.DeleteTagAsync(It.IsAny<Tag>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTagDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteUserTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            tagRepository.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)null);

            var handler = new DeleteUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.TagNotFound);

            tagRepository.Verify(r => r.DeleteTagAsync(It.IsAny<Tag>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteUserTagHandler>>();

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

            tagRepository.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(tag);

            var handler = new DeleteUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.Forbidden);

            tagRepository.Verify(r => r.DeleteTagAsync(It.IsAny<Tag>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteTag_WhenUserOwnsTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<DeleteUserTagHandler>>();

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

            tagRepository.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(tag);

            var handler = new DeleteUserTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            tagRepository.Verify(r => r.DeleteTagAsync(tag), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
