using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly AppDbContext dbContext;
        public FriendshipRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Friendship?> GetFriendshipAsync(int userId, int friendId)
        {
            return dbContext.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.RequesterId == userId && f.AddresseeId == friendId) ||
                    (f.RequesterId == friendId && f.AddresseeId == userId));
        }

        public async Task<List<Friendship>> GetAllFriendshipsByUserIdAsync(int userId)
        {
            return await dbContext.Friendships
                .AsNoTracking()
                .Where(f => f.RequesterId == userId || f.AddresseeId == userId)
                .ToListAsync();
        }

        public async Task<List<Friendship>> GetAcceptedFriendshipsAsync(int userId)
        {
            return await dbContext.Friendships
                .AsNoTracking()
                .Where(f => (f.RequesterId == userId || f.AddresseeId == userId)
                && f.Status == FriendshipStatus.Accepted)
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

        public Task RemoveFriendshipAsync(Friendship friendship)
        {
            dbContext.Friendships.Remove(friendship);

            return Task.CompletedTask;
        }
    }
}
