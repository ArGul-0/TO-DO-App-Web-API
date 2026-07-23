using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Notes.CreateNewNote;
using ToDoApp.Application.UseCases.Users;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Notes
{
    public sealed class CreateNewNoteHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateNewNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            userRepository.Setup(repo => repo.GetUserWithNotesAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            var handler = new CreateNewNoteHandler(
                userRepository.Object,
                logger.Object,
                unitOfWork.Object
            );

            var request = new CreateNewNoteRequest(
                Title: "Test Note",
                Content: "This is a test note.",
                IsDone: false
            );

            // Act
            var result = await handler.Handle(request, 1);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UsersErrors.UserNotFound);
            
            unitOfWork.Verify(
                u => u.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateNote_WhenRequestIsValid()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateNewNoteHandler>>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var user = new User(
                "John Doe",
                new Email("john.doe@example.com"),
                "Example of hashed password"
            );

            userRepository.Setup(repo => repo.GetUserWithNotesAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var handler = new CreateNewNoteHandler(
                userRepository.Object,
                logger.Object,
                unitOfWork.Object
            );

            var request = new CreateNewNoteRequest(
                Title: "Test Note",
                Content: "This is a test note.",
                IsDone: false
            );

            // Act
            var result = await handler.Handle(request, 1);

            // Assert
            result.IsSuccess.Should().BeTrue()
                ;
            result.Value.Title.Should().Be(request.Title);
            result.Value.Content.Should().Be(request.Content);
            result.Value.IsDone.Should().Be(request.IsDone);

            user.Notes.Should().ContainSingle(note =>
                note.Title == request.Title &&
                note.Content == request.Content &&
                note.IsDone == request.IsDone);

            unitOfWork.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
    }
}
