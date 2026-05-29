using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        public Task<List<Friendship>> GetFriendshipsByUserIdAsync(int userId);
        public Task AddFriendshipAsync(Friendship friendship);
        public Task<bool> FriendshipExistsAsync(int userId, int friendId);
        public Task RemoveFriendshipAsync(int userId, int friendId);
    }
}
