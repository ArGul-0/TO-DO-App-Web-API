using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes.DeleteUserNote;
using ToDoApp.Application.UseCases.Notes;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class DeleteUserNoteHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoteDoesNotExist()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<DeleteUserNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            notesRepository.Setup(r => r.GetNoteById(It.IsAny<int>()))
                .ReturnsAsync((Note?)null);

            var handler = new DeleteUserNoteHandler(
                notesRepository.Object,
                unitOfWork.Object,
                notesAuth.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.NoteNotFound);

            notesRepository.Verify(r => r.DeleteNoteAsync(It.IsAny<int>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnNote()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<DeleteUserNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            var note = new Note("Title", "Content", false, 2);

            notesRepository.Setup(r => r.GetNoteById(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(s => s.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(false);

            var handler = new DeleteUserNoteHandler(
                notesRepository.Object,
                unitOfWork.Object,
                notesAuth.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.Forbidden);

            notesRepository.Verify(r => r.DeleteNoteAsync(It.IsAny<int>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteNote_WhenUserOwnsNote()
        {
            // Arrange
            var notesRepository = new Mock<INoteRepository>();
            var logger = new Mock<ILogger<DeleteUserNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var notesAuth = new Mock<INotesAuthorizationService>();

            var note = new Note("Title", "Content", false, 1);

            notesRepository.Setup(r => r.GetNoteById(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(s => s.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(true);

            notesRepository.Setup(r => r.DeleteNoteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var handler = new DeleteUserNoteHandler(
                notesRepository.Object,
                unitOfWork.Object,
                notesAuth.Object,
                logger.Object
            );

            // Act
            var result = await handler.Handle(1, 1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            notesRepository.Verify(r => r.DeleteNoteAsync(1), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
