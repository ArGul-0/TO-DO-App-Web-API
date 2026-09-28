using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Notes.GetUserNotesByTag;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class GetUserNotesByTagHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnUserNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<GetUserNotesByTagHandler>>();

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new GetUserNotesByTagHandler(noteRepository.Object,
                userRepository.Object,
                tagRepository.Object,
                logger.Object);

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            userRepository.Verify(repo => repo.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
            tagRepository.Verify(repo => repo.GetTagByIdAsync(It.IsAny<int>()), Times.Never);
            noteRepository.Verify(repo => repo.GetAllNotesByTagAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnTagNotFound_WhenTagDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<GetUserNotesByTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            tagRepository.Setup(repo => repo.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)null);

            var handler = new GetUserNotesByTagHandler(noteRepository.Object,
                userRepository.Object,
                tagRepository.Object,
                logger.Object);

            // Act
            var result = await handler.Handle(1, user.Id);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.TagNotFound);

            userRepository.Verify(repo => repo.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
            tagRepository.Verify(repo => repo.GetTagByIdAsync(It.IsAny<int>()), Times.Once);
            noteRepository.Verify(repo => repo.GetAllNotesByTagAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnForbidden_WhenTagDoesNotBelongToUser()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<GetUserNotesByTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            var tag = new Tag("Test Tag", 2);

            userRepository.Setup(repo => repo.GetUserByIdAsync(1))
                .ReturnsAsync(user);

            tagRepository.Setup(repo => repo.GetTagByIdAsync(1))
                .ReturnsAsync(tag);

            var handler = new GetUserNotesByTagHandler(noteRepository.Object,
                userRepository.Object,
                tagRepository.Object,
                logger.Object);

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.Forbidden);

            userRepository.Verify(repo => repo.GetUserByIdAsync(1), Times.Once);
            tagRepository.Verify(repo => repo.GetTagByIdAsync(1), Times.Once);
            noteRepository.Verify(repo => repo.GetAllNotesByTagAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotes_WhenTagExistsAndBelongsToUser()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<GetUserNotesByTagHandler>>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            var tag = new Tag("Test Tag", user.Id);

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            tagRepository.Setup(repo => repo.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(tag);

            noteRepository.Setup(repo => repo.GetAllNotesByTagAndUserIdAsync(tag.Id, user.Id))
                .ReturnsAsync(new List<Note>
                {
                    new Note("Test Note 1", "Test Content 1", false, user.Id),
                    new Note("Test Note 2", "Test Content 2", false, user.Id),
                });

            var handler = new GetUserNotesByTagHandler(noteRepository.Object,
                userRepository.Object,
                tagRepository.Object,
                logger.Object);

            // Act
            var result = await handler.Handle(tag.Id, user.Id);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Value.Should().HaveCount(2);
            result.Value.Should().ContainSingle(note => note.Title == "Test Note 1");
            result.Value.Should().ContainSingle(note => note.Title == "Test Note 2");

            userRepository.Verify(repo => repo.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
            tagRepository.Verify(repo => repo.GetTagByIdAsync(It.IsAny<int>()), Times.Once);
            noteRepository.Verify(repo => repo.GetAllNotesByTagAndUserIdAsync(tag.Id, user.Id), Times.Once);
        }
    }
}
