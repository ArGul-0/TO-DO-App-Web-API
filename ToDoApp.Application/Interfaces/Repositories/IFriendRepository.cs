using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface IFriendRepository
    {
        public Task AddFriendshipAsync(Friendship friendship);
        public Task UpdateFriendshipAsync(Friendship friendship);
        public Task<bool> FriendshipExistsAsync(int userId, int friendId);
        public Task RemoveFriendshipAsync(int userId, int friendId);
    }
}
