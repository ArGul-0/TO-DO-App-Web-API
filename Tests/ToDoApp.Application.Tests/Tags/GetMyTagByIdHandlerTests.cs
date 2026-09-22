using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Tags.GetMyTagById;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Tags
{
    public sealed class GetMyTagByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var logger = new Mock<ILogger<GetMyTagByIdHandler>>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new GetMyTagByIdHandler(
                userRepository.Object,
                tagRepository.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            tagRepository.Verify(r => r.GetTagByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTagDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var logger = new Mock<ILogger<GetMyTagByIdHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            tagRepository.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)null);

            var handler = new GetMyTagByIdHandler(
                userRepository.Object,
                tagRepository.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.TagNotFound);

            tagRepository.Verify(
                r => r.GetTagByIdAsync(It.IsAny<int>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var logger = new Mock<ILogger<GetMyTagByIdHandler>>();

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

            var handler = new GetMyTagByIdHandler(
                userRepository.Object,
                tagRepository.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.Forbidden);
        }

        [Fact]
        public async Task Handle_ShouldReturnTag_WhenUserOwnsTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var logger = new Mock<ILogger<GetMyTagByIdHandler>>();

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

            var handler = new GetMyTagByIdHandler(
                userRepository.Object,
                tagRepository.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(tagId: 1, userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<TagDto>();
            result.Value.Name.Should().Be("Test Tag");

            tagRepository.Verify(
                r => r.GetTagByIdAsync(It.IsAny<int>()),
                Times.Once);
        }
    }
}
