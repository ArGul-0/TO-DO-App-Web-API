using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly AppDbContext dbContext;
        public FriendshipRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<Friendship>> GetFriendshipsByUserIdAsync(int userId)
        {
            return dbContext.Friendships
                .Where(f => f.RequesterId == userId || f.AddresseeId == userId)
                .ToListAsync();
        }

        public async Task AddFriendshipAsync(Friendship friendship)
        {
            await dbContext.Friendships.AddAsync(friendship);
        }

        public async Task<bool> FriendshipExistsAsync(int userId, int friendId)
        {
            return await dbContext.Friendships.AnyAsync(f =>
                (f.RequesterId == userId && f.AddresseeId == friendId) ||
                (f.RequesterId == friendId && f.AddresseeId == userId));
        }

        public async Task RemoveFriendshipAsync(int userId, int friendId)
        {
            var friendship = await dbContext.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == userId && f.AddresseeId == friendId) ||
                (f.RequesterId == friendId && f.AddresseeId == userId));

            if (friendship != null)
            {
                dbContext.Friendships.Remove(friendship);
            }
        }
    }
}
