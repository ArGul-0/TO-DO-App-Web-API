using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes.UpdateUserNote;
using ToDoApp.Application.UseCases.Notes;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class UpdateUserNoteHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoteNotFound()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<UpdateUserNoteHandler>>();

            notesRepository.Setup(r => r.GetNoteByIdWithTracking(It.IsAny<int>()))
                .ReturnsAsync((Note?)null);

            var handler = new UpdateUserNoteHandler(notesRepository.Object, unitOfWork.Object, notesAuth.Object, logger.Object);

            var request = new UpdateUserNoteRequest("Test Title","This is a test note", false);

            // Act
            var result = await handler.Handle(request, 1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.NoteNotFound);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnNote()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<UpdateUserNoteHandler>>();

            var note = new Note("Old Title","Old Content", false, userId: 2);

            notesRepository.Setup(r => r.GetNoteByIdWithTracking(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(s => s.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>())).Returns(false);

            var handler = new UpdateUserNoteHandler(notesRepository.Object, unitOfWork.Object, notesAuth.Object, logger.Object);

            var request = new UpdateUserNoteRequest("New Title","New Content", true);

            // Act
            var result = await handler.Handle(request, 1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.Forbidden);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUpdateNote_WhenUserOwnsNote()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<UpdateUserNoteHandler>>();

            var note = new Note("Old Title","Old Content", false, userId: 1);

            notesRepository.Setup(r => r.GetNoteByIdWithTracking(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(s => s.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>())).Returns(true);

            var handler = new UpdateUserNoteHandler(notesRepository.Object, unitOfWork.Object, notesAuth.Object, logger.Object);

            var request = new UpdateUserNoteRequest("New Title","New Content", true);

            // Act
            var result = await handler.Handle(request, 1, 1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            note.Title.Should().Be(request.Title);
            note.Content.Should().Be(request.Content);
            note.IsDone.Should().Be(request.IsDone);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
