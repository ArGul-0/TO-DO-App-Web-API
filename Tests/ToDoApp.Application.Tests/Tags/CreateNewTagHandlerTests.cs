using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Tags.CreateNewTag;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Tags
{
    public sealed class CreateNewTagHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<CreateNewTagHandler>>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new CreateNewTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            var request = new CreateNewTagRequest(
                Name: "Test Tag"
            );

            // Act
            var result = await handler.Handle(request, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            tagRepository.Verify(r => r.AddTagAsync(It.IsAny<Tag>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateTag_WhenUserExists()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var logger = new Mock<ILogger<CreateNewTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            tagRepository.Setup(r => r.AddTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            var handler = new CreateNewTagHandler(
                userRepository.Object,
                tagRepository.Object,
                unitOfWork.Object,
                logger.Object
            );

            var request = new CreateNewTagRequest(
                Name: "Test Tag"
            );

            // Act
            var result = await handler.Handle(request, userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<TagDto>();
            result.Value.Name.Should().Be("Test Tag");

            tagRepository.Verify(
                r => r.AddTagAsync(
                    It.Is<Tag>(tag =>
                        tag.Name == "Test Tag")),
                Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
