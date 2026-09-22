using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Tags.GetAllMyTags;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Tags
{
    public sealed class GetAllMyTagsHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var tagRepository = new Mock<ITagRepository>();
            var userRepository = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<GetAllMyTagsHandler>>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new GetAllMyTagsHandler(
                tagRepository.Object,
                userRepository.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            tagRepository.Verify(r => r.GetAllTagsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnTags_WhenUserExists()
        {
            // Arrange
            var tagRepository = new Mock<ITagRepository>();
            var userRepository = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<GetAllMyTagsHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var tags = new List<Tag>
            {
                new Tag("Test Tag", ownerId: 1),
                new Tag("Test Tag", ownerId: 1)
            };

            tagRepository.Setup(r => r.GetAllTagsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(tags);

            var handler = new GetAllMyTagsHandler(
                tagRepository.Object,
                userRepository.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<List<TagDto>>();
            result.Value.Should().HaveCount(2);

            result.Value[0].Name.Should().Be("Test Tag");
            result.Value[1].Name.Should().Be("Test Tag");

            tagRepository.Verify(r => r.GetAllTagsByUserIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
