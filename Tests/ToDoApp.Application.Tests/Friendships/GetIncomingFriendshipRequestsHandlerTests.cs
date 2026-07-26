using FluentAssertions;
using Moq;
using ToDoApp.Domain.ValueObjects;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Tests
{
    public sealed class GetIncomingFriendshipRequestsHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnDtos_WhenFriendshipsExist()
        {
            // Arrange
            var friendshipRepo = new Mock<IFriendshipRepository>();

            var friendship1 = new Friendship(1, 2);
            var friendship2 = new Friendship(3, 2);

            var requester1 = new User("John Doe", new Email("john.doe@example.com"), "Example of hashed password");
            var requester2 = new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password");
            var addressee = new User("Target User", new Email("target.user@example.com"), "Example of hashed password");

            typeof(Friendship).GetProperty("Requester", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(friendship1, requester1);
            typeof(Friendship).GetProperty("Addressee", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(friendship1, addressee);

            typeof(Friendship).GetProperty("Requester", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(friendship2, requester2);
            typeof(Friendship).GetProperty("Addressee", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(friendship2, addressee);

            friendshipRepo.Setup(r => r.GetIncomingFriendshipsRequestsAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Friendship> { friendship1, friendship2 });

            var handler = new GetIncomingFriendshipRequestsHandler(friendshipRepo.Object);

            // Act
            var result = await handler.Handle(2);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            result.Value.Select(x => x.RequesterUsername).Should().Contain(new[] { "John Doe", "Jane Smith" });
        }
    }
}
