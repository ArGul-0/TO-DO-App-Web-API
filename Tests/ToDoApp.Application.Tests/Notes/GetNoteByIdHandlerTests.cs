using FluentAssertions;
using Moq;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes.GetNoteById;
using ToDoApp.Application.UseCases.Notes;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class GetNoteByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var userRepository = new Mock<IUserRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new GetNoteByIdHandler(notesRepository.Object, userRepository.Object, notesAuth.Object);

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoteNotFound()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var userRepository = new Mock<IUserRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            var user = new User("John", new Email("john@example.com"), "pwd");
            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(r => r.GetNoteWithOwnerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)null);

            var handler = new GetNoteByIdHandler(notesRepository.Object, userRepository.Object, notesAuth.Object);

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.NoteNotFound);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCannotSeeNote()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var userRepository = new Mock<IUserRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            var user = new User("John", new Email("john@example.com"), "Example of hashed password");
            var note = new Note("Test Note", "This is a test note", false, userId: 2);

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(r => r.GetNoteWithOwnerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(s => s.CanSeeNote(user, note)).Returns(false);

            var handler = new GetNoteByIdHandler(notesRepository.Object, userRepository.Object, notesAuth.Object);

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.Forbidden);
        }

        [Fact]
        public async Task Handle_ShouldReturnNote_WhenUserCanSeeNote()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var userRepository = new Mock<IUserRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            var user = new User("John", new Email("john@example.com"), "Example of hashed password");
            var note = new Note("Test Note", "This is a test note", true, userId: 1);

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(r => r.GetNoteWithOwnerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(s => s.CanSeeNote(user, note)).Returns(true);

            var handler = new GetNoteByIdHandler(notesRepository.Object, userRepository.Object, notesAuth.Object);

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Title.Should().Be(note.Title);
            result.Value.Content.Should().Be(note.Content);
            result.Value.IsDone.Should().Be(note.IsDone);
        }
    }
}
