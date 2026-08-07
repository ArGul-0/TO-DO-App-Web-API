using FluentAssertions;
using Moq;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Application.Tests.Users
{
    public sealed class GetAllUsersHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnAllUsers_WhenUsersExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();

            var users = new List<User>
            {
                new User("John Doe", new Email("john@example.com"), "Example of hashed password"),
                new User("Jane Smith", new Email("jane.smith@example.com"), "Example of hashed password")
            };
            userRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            var usersDtos = users.Select(u => u.ToDto()).ToList();

            var handler = new GetAllUsersHandler(userRepository.Object);

            // Act
            var result = await handler.Handle();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Users.Should().HaveCount(2);
            result.Value.Users.Should().BeEquivalentTo(usersDtos);

            userRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(new List<User>());

            var handler = new GetAllUsersHandler(userRepository.Object);

            // Act
            var result = await handler.Handle();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Users.Should().BeEmpty();

            userRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
        }
    }
}
