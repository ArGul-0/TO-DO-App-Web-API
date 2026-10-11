using FluentAssertions;
using Moq;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Security.Notes
{
    public sealed class NotesAuthorizationServiceTests
    {
        [Fact]
        public async Task CanSeeNote_ShouldReturnTrue_WhenUserOwnsNote()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var user = new User("John Doe", new Email("john@example.com"), "Example of hashed password");

            user.ChangeAccountVisibility(AccountVisibility.Private);

            var note = new Note("Test Note", "This is a test note", false, user.Id);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(user, note);

            // Assert
            result.Should().BeTrue();

            friendshipRepository.Verify(
               repo => repo.GetFriendshipAsync(
                   It.IsAny<int>(),
                   It.IsAny<int>()),
               Times.Never);
        }

        [Fact]
        public async Task CanSeeNote_ShouldReturnTrue_WhenNoteOwnerHasPublicAccount()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            SetUserId(currentUser, 1);

            var noteOwner = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            SetUserId(noteOwner, 2);

            noteOwner.ChangeAccountVisibility(AccountVisibility.Public);

            var note = new Note("Test Note", "This is a test note", false, noteOwner.Id);

            SetNoteUser(note, noteOwner);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(currentUser, note);

            // Assert
            result.Should().BeTrue();

            friendshipRepository.Verify(
               repo => repo.GetFriendshipAsync(
                   It.IsAny<int>(),
                   It.IsAny<int>()),
               Times.Never);
        }

        [Fact]
        public async Task CanSeeNote_ShouldReturnFalse_WhenNoteOwnerHasPrivateAccount()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            SetUserId(currentUser, 1);

            var noteOwner = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            SetUserId(noteOwner, 2);

            noteOwner.ChangeAccountVisibility(AccountVisibility.Private);

            var note = new Note("Test Note", "This is a test note", false, noteOwner.Id);

            SetNoteUser(note, noteOwner);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(currentUser, note);

            // Assert
            result.Should().BeFalse();

            friendshipRepository.Verify(
               repo => repo.GetFriendshipAsync(
                   It.IsAny<int>(),
                   It.IsAny<int>()),
               Times.Never);
        }

        [Fact]
        public async Task CanSeeNote_ShouldReturnTrue_WhenUsersAreFriendsAndAccountIsOnlyForFriends()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            SetUserId(currentUser, 1);

            var noteOwner = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            SetUserId(noteOwner, 2);

            noteOwner.ChangeAccountVisibility(AccountVisibility.OnlyForFriends);

            var note = new Note("Test Note", "This is a test note", false, noteOwner.Id);

            SetNoteUser(note, noteOwner);

            var friendship = new Friendship(
                currentUser.Id,
                noteOwner.Id);

            friendship.Accept();

            friendshipRepository
                .Setup(repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id))
                .ReturnsAsync(friendship);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(currentUser, note);

            // Assert
            result.Should().BeTrue();

            friendshipRepository.Verify(
                repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id),
                Times.Once);
        }

        [Fact]
        public async Task CanSeeNote_ShouldReturnFalse_WhenFriendshipIsPending()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            SetUserId(currentUser, 1);

            var noteOwner = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            SetUserId(noteOwner, 2);

            noteOwner.ChangeAccountVisibility(AccountVisibility.OnlyForFriends);

            var note = new Note("Test Note", "This is a test note", false, noteOwner.Id);

            SetNoteUser(note, noteOwner);

            var friendship = new Friendship(
                currentUser.Id,
                noteOwner.Id);

            friendshipRepository
                .Setup(repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id))
                .ReturnsAsync(friendship);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(currentUser, note);

            // Assert
            result.Should().BeFalse();

            friendshipRepository.Verify(
                repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id),
                Times.Once);
        }

        [Fact]
        public async Task CanSeeNote_ShouldReturnFalse_WhenFriendshipIsRejected()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            SetUserId(currentUser, 1);

            var noteOwner = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            SetUserId(noteOwner, 2);

            noteOwner.ChangeAccountVisibility(AccountVisibility.OnlyForFriends);

            var note = new Note("Test Note", "This is a test note", false, noteOwner.Id);

            SetNoteUser(note, noteOwner);

            var friendship = new Friendship(
                currentUser.Id,
                noteOwner.Id);

            friendship.Reject();

            friendshipRepository
                .Setup(repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id))
                .ReturnsAsync(friendship);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(currentUser, note);

            // Assert
            result.Should().BeFalse();

            friendshipRepository.Verify(
                repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id),
                Times.Once);
        }

        [Fact]
        public async Task CanSeeNote_ShouldReturnFalse_WhenFriendshipDoesNotExist()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User("John Doe", new Email("john@example.com"), "Example of hashed password");
            SetUserId(currentUser, 1);

            var noteOwner = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            SetUserId(noteOwner, 2);

            noteOwner.ChangeAccountVisibility(AccountVisibility.OnlyForFriends);

            var note = new Note("Test Note", "This is a test note", false, noteOwner.Id);

            SetNoteUser(note, noteOwner);

            friendshipRepository
                .Setup(repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id))
                .ReturnsAsync((Friendship)null!);

            // Act
            var result = await notesAuthorizationService.CanSeeNote(currentUser, note);

            // Assert
            result.Should().BeFalse();

            friendshipRepository.Verify(
                repo => repo.GetFriendshipAsync(
                    currentUser.Id,
                    noteOwner.Id),
                Times.Once);
        }

        [Fact]
        public async Task FilterVisibleNotes_ShouldReturnOnlyVisibleNotes()
        {
            // Arrange
            var friendshipRepository = new Mock<IFriendshipRepository>();

            var notesAuthorizationService = new NotesAuthorizationService(friendshipRepository.Object);

            var currentUser = new User(
                "John Doe",
                new Email("john@example.com"),
                "Example of hashed password");

            SetUserId(currentUser, 1);

            var ownNote = new Note(
                "Own note",
                "My private note",
                false,
                currentUser.Id);

            var publicOwner = new User(
                "Jane Smith",
                new Email("jane@example.com"),
                "Example of hashed password");

            SetUserId(publicOwner, 2);
            publicOwner.ChangeAccountVisibility(AccountVisibility.Public);

            var publicNote = new Note(
                "Public note",
                "Anyone can see this",
                false,
                publicOwner.Id);

            SetNoteUser(publicNote, publicOwner);

            var privateOwner = new User(
                "Alex Brown",
                new Email("alex@example.com"),
                "Example of hashed password");

            SetUserId(privateOwner, 3);
            privateOwner.ChangeAccountVisibility(AccountVisibility.Private);

            var privateNote = new Note(
                "Private note",
                "Not visible to others",
                false,
                privateOwner.Id);

            SetNoteUser(privateNote, privateOwner);

            var notes = new List<Note>
            {
                ownNote,
                publicNote,
                privateNote
            };

            // Act
            var result = await notesAuthorizationService.FilterVisibleNotes(currentUser, notes);

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainInOrder(ownNote, publicNote);
            result.Should().NotContain(privateNote);

            friendshipRepository.Verify(
                repo => repo.GetFriendshipAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>()),
                Times.Never);
        }



        private static void SetUserId(User user, int id)
        {
            typeof(User)
                .GetProperty(nameof(User.Id))!
                .SetValue(user, id);
        }

        private static void SetNoteUser(Note note, User user)
        {
            typeof(Note)
                .GetProperty(nameof(Note.User))!
                .SetValue(note, user);
        }
    }
}
