using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes;
using ToDoApp.Application.UseCases.Notes.DetachTagFromNote;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class DetachTagFromNoteHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var notesRepository = new Mock<INoteRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<DetachTagFromNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new DetachTagFromNoteHandler(
                userRepository.Object,
                notesRepository.Object,
                notesAuth.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(
                noteId: 1,
                tagId: 1,
                userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            notesRepository.Verify(
                r => r.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Never);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoteDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var notesRepository = new Mock<INoteRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<DetachTagFromNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(
                    r => r.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)null);

            var handler = new DetachTagFromNoteHandler(
                userRepository.Object,
                notesRepository.Object,
                notesAuth.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(
                noteId: 1,
                tagId: 1,
                userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.NoteNotFound);

            notesAuth.Verify(
                s => s.IsUserOwnsNote(
                    It.IsAny<int>(),
                    It.IsAny<Note>()),
                Times.Never);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnNote()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var notesRepository = new Mock<INoteRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<DetachTagFromNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            var note = new Note(
                "Test Note",
                "Test Content",
                false,
                2
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(
                    r => r.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(
                    s => s.IsUserOwnsNote(
                        It.IsAny<int>(),
                        It.IsAny<Note>()))
                .Returns(false);

            var handler = new DetachTagFromNoteHandler(
                userRepository.Object,
                notesRepository.Object,
                notesAuth.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(
                noteId: 1,
                tagId: 1,
                userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.Forbidden);

            notesAuth.Verify(
                s => s.IsUserOwnsNote(
                    1,
                    note),
                Times.Once);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTagIsNotAttachedToNote()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var notesRepository = new Mock<INoteRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<DetachTagFromNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            var note = new Note(
                "Test Note",
                "Test Content",
                false,
                1
            );

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(
                    r => r.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(
                    s => s.IsUserOwnsNote(
                        It.IsAny<int>(),
                        It.IsAny<Note>()))
                .Returns(true);

            var handler = new DetachTagFromNoteHandler(
                userRepository.Object,
                notesRepository.Object,
                notesAuth.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(
                noteId: 1,
                tagId: 1,
                userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(
                DetachTagFromNoteErrors.TagNotAttachedToNote);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDetachTagFromNote_WhenTagIsAttachedToNote()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var notesRepository = new Mock<INoteRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<DetachTagFromNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            var note = new Note(
                "Test Note",
                "Test Content",
                false,
                1
            );

            var noteTag = new NoteTag(
                noteId: 1,
                tagId: 1
            );

            note.AddTag(noteTag);

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(
                    r => r.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(
                    s => s.IsUserOwnsNote(
                        It.IsAny<int>(),
                        It.IsAny<Note>()))
                .Returns(true);

            var handler = new DetachTagFromNoteHandler(
                userRepository.Object,
                notesRepository.Object,
                notesAuth.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(
                noteId: 1,
                tagId: 1,
                userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            note.NoteTags.Should().NotContain(noteTag);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldDetachOnlySpecifiedTag_WhenNoteHasMultipleTags()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var notesRepository = new Mock<INoteRepository>();
            var notesAuth = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<DetachTagFromNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            var note = new Note(
                "Test Note",
                "Test Content",
                false,
                1
            );

            var firstNoteTag = new NoteTag(
                noteId: 1,
                tagId: 1
            );

            var secondNoteTag = new NoteTag(
                noteId: 1,
                tagId: 2
            );

            note.AddTag(firstNoteTag);
            note.AddTag(secondNoteTag);

            userRepository.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            notesRepository.Setup(
                    r => r.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync(note);

            notesAuth.Setup(
                    s => s.IsUserOwnsNote(
                        It.IsAny<int>(),
                        It.IsAny<Note>()))
                .Returns(true);

            var handler = new DetachTagFromNoteHandler(
                userRepository.Object,
                notesRepository.Object,
                notesAuth.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(
                noteId: 1,
                tagId: 1,
                userId: 1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            note.NoteTags.Should().HaveCount(1);
            note.NoteTags.Should().NotContain(firstNoteTag);
            note.NoteTags.Should().Contain(secondNoteTag);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Once);
        }
    }
}