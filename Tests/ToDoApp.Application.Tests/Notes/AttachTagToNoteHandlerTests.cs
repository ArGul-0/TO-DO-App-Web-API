using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Notes;
using ToDoApp.Application.UseCases.Notes.AttachTagToNote;
using ToDoApp.Application.UseCases.Tags;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class AttachTagToNoteHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );

            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);

            userRepository.Verify(
                repo => repo.GetUserByIdAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
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
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            noteRepository.Setup(repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)null);

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );
            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.NoteNotFound);

            noteRepository.Verify(
                repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
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
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)user);

            var note = new Note(
                "Test Note",
                "This is a test note.",
                false,
                userId: 2
            );

            noteRepository.Setup(repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)note);

            notesAuthorizationService.Setup(service => service.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(false);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );
            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(NotesErrors.Forbidden);

            noteRepository.Verify(
                repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
                Times.Never);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTagDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)user);

            var note = new Note(
                "Test Note",
                "This is a test note.",
                false,
                userId: 1
            );

            noteRepository.Setup(repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)note);

            tagRepository.Setup(repo => repo.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)null);

            notesAuthorizationService.Setup(service => service.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(true);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );
            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.TagNotFound);

            noteRepository.Verify(
                repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
                Times.Once);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotOwnTag()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)user);

            var note = new Note(
                "Test Note",
                "This is a test note.",
                false,
                userId: user.Id
                );

            noteRepository.Setup(repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)note);

            var tag = new Tag(
                "Test Tag",
                ownerId: 0
            );

            tagRepository.Setup(repo => repo.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)tag);

            notesAuthorizationService.Setup(service => service.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(true);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );
            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TagsErrors.Forbidden);

            noteRepository.Verify(
                repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
                Times.Once);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTagAlreadyAttachedToNote()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)user);

            var note = new Note(
                "Test Note",
                "This is a test note.",
                false,
                userId: user.Id
                );

            var noteTag = new NoteTag(
                noteId: 1,
                tagId: 1
            );

            note.AddTag(noteTag);

            noteRepository.Setup(repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)note);

            var tag = new Tag(
                "Test Tag",
                ownerId: 1
            );

            tagRepository.Setup(repo => repo.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)tag);

            notesAuthorizationService.Setup(service => service.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(true);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );
            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DetachTagFromNoteHandlerErrors.TagAlreadyAttachedToNote);

            noteRepository.Verify(
                repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
                Times.Once);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldAttachTagToNote_WhenAllConditionsAreValid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var noteRepository = new Mock<INoteRepository>();
            var tagRepository = new Mock<ITagRepository>();
            var notesAuthorizationService = new Mock<INotesAuthorizationService>();
            var logger = new Mock<ILogger<AttachTagToNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)user);

            var note = new Note(
                "Test Note",
                "This is a test note.",
                false,
                userId: user.Id
            );

            noteRepository.Setup(repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()))
                .ReturnsAsync((Note?)note);

            var tag = new Tag(
                "Test Tag",
                ownerId: 1
                );

            tagRepository.Setup(repo => repo.GetTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Tag?)tag);

            notesAuthorizationService.Setup(service => service.IsUserOwnsNote(It.IsAny<int>(), It.IsAny<Note>()))
                .Returns(true);

            var handler = new AttachTagToNoteHandler(
                userRepository.Object,
                noteRepository.Object,
                tagRepository.Object,
                notesAuthorizationService.Object,
                logger.Object,
                unitOfWork.Object
            );
            // Act
            var result = await handler.Handle(noteId: 1, tagId: 1, userId: 1);

            // Assert
            result.IsFailure.Should().BeFalse();

            note.NoteTags.Should().ContainSingle();
            note.NoteTags.Single().NoteId.Should().Be(1);
            note.NoteTags.Single().TagId.Should().Be(1);

            noteRepository.Verify(
                repo => repo.GetNoteByIdWithTrackingIncludeNoteTagsAsync(It.IsAny<int>()),
                Times.Once);

            tagRepository.Verify(
                repo => repo.GetTagByIdAsync(It.IsAny<int>()),
                Times.Once);

            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Once);
        }
    }
}
