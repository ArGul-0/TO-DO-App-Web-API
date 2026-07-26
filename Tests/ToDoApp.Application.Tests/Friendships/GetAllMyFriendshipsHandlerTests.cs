using FluentAssertions;
using Moq;
using System.Reflection;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests
{
    public sealed class GetAllMyFriendshipsHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnDtos_WhenFriendshipsExist()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();

            var f1 = new Friendship(1, 2);
            var f2 = new Friendship(3, 1);

            var user1 = new User("John Doe", new Email("john.doe@example.com"), "Example of hashed password");
            var user2 = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");

            // set related users so mapping can access usernames
            typeof(Friendship).GetProperty("Requester", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(f1, user1);
            typeof(Friendship).GetProperty("Addressee", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(f1, user2);

            typeof(Friendship).GetProperty("Requester", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(f2, user2);
            typeof(Friendship).GetProperty("Addressee", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(f2, user1);

            friendshipRepo.Setup(r => r.GetAllFriendshipsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Friendship> { f1, f2 });

            var handler = new GetAllMyFriendshipsHandler(friendshipRepo.Object);

            // Act
            var result = await handler.Handle(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            result.Value.Select(x => x.RequesterUsername).Should().Contain(new[] { "John Doe", "Jane Smith" });
        }
    }
}
