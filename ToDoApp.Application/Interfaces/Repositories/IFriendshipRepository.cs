using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        public Task<Friendship?> GetFriendshipAsync(int userId, int friendId);
        public Task<List<Friendship>> GetAllFriendshipsByUserIdAsync(int userId);
        public Task<List<Friendship>> GetAcceptedFriendshipsAsync(int userId);
        public Task AddFriendshipAsync(Friendship friendship);
        public Task<bool> FriendshipExistsAsync(int userId, int friendId);
        public void RemoveFriendship(Friendship friendship);
    }
}
