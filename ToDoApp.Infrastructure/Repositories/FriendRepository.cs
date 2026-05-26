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
    }
}
