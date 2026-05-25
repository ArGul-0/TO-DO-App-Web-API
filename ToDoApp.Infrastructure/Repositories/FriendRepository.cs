using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Infrastructure.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly AppDbContext dbContext;
        public FriendRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SendFriendRequest(int requesterId, int addresseeId)
        {

        }
    }
}
