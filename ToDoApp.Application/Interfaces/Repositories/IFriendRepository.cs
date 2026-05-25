namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface IFriendRepository
    {
        public Task AddFriendRequestAsync(int requesterId, int addresseeId);
    }
}
