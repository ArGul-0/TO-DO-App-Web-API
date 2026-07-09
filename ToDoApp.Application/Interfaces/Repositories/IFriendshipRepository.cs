using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        /// <summary>
        /// Gets a friendship between two users by their IDs.
        /// </summary>
        /// <param name="userId">The ID of the first user.</param>
        /// <param name="friendId">The ID of the second user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the friendship if it exists; otherwise, null.</returns>
        public Task<Friendship?> GetFriendshipAsync(int userId, int friendId);
        /// <summary>
        /// Gets all friendships for a specific user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of friendships for the user.</returns>
        public Task<List<Friendship>> GetAllFriendshipsByUserIdAsync(int userId);
        /// <summary>
        /// Gets all incoming friendship requests for a specific user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of incoming friendship requests for the user.</returns>
        public Task<List<Friendship>> GetIncomingFriendshipsRequestsAsync(int userId);
        /// <summary>
        /// Gets all outgoing friendship requests for a specific user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of outgoing friendship requests for the user.</returns>
        public Task<List<Friendship>> GetOutgoingFriendshipsRequestsAsync(int userId);
        /// <summary>
        /// Gets all accepted friendships for a specific user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of accepted friendships for the user.</returns>
        public Task<List<Friendship>> GetAcceptedFriendshipsAsync(int userId);
        /// <summary>
        /// Adds a new friendship to the repository.
        /// </summary>
        /// <param name="friendship">The friendship to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddFriendshipAsync(Friendship friendship);
        /// <summary>
        /// Checks if a friendship exists between two users by their IDs.
        /// </summary>
        /// <param name="userId">The ID of the first user.</param>
        /// <param name="friendId">The ID of the second user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the friendship exists; otherwise, false.</returns>
        public Task<bool> FriendshipExistsAsync(int userId, int friendId);
        /// <summary>
        /// Deletes a friendship from the repository.
        /// </summary>
        /// <param name="friendship">The friendship to delete.</param>
        public void DeleteFriendship(Friendship friendship);
    }
}
