using FluentAssertions;
using Moq;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes.GetAllNotes;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class GetAllNotesHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var userRepository = new Mock<IUserRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            notesRepository.Setup(r => r.GetAllNotesWithOwnersAsync())
                .ReturnsAsync(new List<Note>());

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new GetAllNotesHandler(notesRepository.Object, userRepository.Object, notesAuth.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);
        }

        [Fact]
        public async Task Handle_ShouldReturnFilteredNotes_WhenUserExists()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var userRepository = new Mock<IUserRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            var note1 = new Note("Test Note 1", "This is a test note 1", false, 1);
            var note2 = new Note("Test Note 2", "This is a test note 2", true, 2);

            notesRepository.Setup(r => r.GetAllNotesWithOwnersAsync())
                .ReturnsAsync(new List<Note> { note1, note2 });

            var user = new User("John", new Email("john@example.com"), "Example of hashed password");
            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesAuth.Setup(s => s.FilterVisibleNotes(user, It.IsAny<List<Note>>()))
                .Returns(new List<Note> { note1 });

            var handler = new GetAllNotesHandler(notesRepository.Object, userRepository.Object, notesAuth.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value[0].Title.Should().Be(note1.Title);
        }
    }
}
