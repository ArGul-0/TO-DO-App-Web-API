namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface IFriendRepository
    {
        public Task SendFriendRequest(int requesterId, int addresseeId);
    }
}
