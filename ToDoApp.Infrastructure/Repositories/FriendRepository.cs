using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly AppDbContext dbContext;
        public FriendRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddFriendshipAsync(Friendship friendship)
        {
            await dbContext.Friendships.AddAsync(friendship);
        }

        public Task UpdateFriendshipAsync(Friendship friendship)
        {
            var existingFriendship = dbContext.Friendships.Find(friendship.Id);

            if(existingFriendship != null)
            {
                dbContext.Entry(existingFriendship).CurrentValues.SetValues(friendship);
            }

            return Task.CompletedTask;
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
