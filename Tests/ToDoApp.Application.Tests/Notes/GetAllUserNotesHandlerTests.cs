using FluentAssertions;
using Moq;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class GetAllUserNotesHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoNotes()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            notesRepository.Setup(r => r.GetAllNotesByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Note>());

            var handler = new GetAllUserNotesHandler(notesRepository.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldReturnNotes_ForUser()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var note = new Note("Test Note", "This is a test note", false, userId: 1);

            notesRepository.Setup(r => r.GetAllNotesByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Note> { note });

            var handler = new GetAllUserNotesHandler(notesRepository.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value[0].Title.Should().Be(note.Title);
        }
    }
}
