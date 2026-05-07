using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Asynchronously retrieves all users from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all users.</returns>
        public Task<List<User>> GetAllUsersAsync();
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user with the specified identifier, or null if no such user exists.</returns>
        public Task<User?> GetUserByIdAsync(int userId);
        /// <summary>
        /// Asynchronously retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
        /// specified username, or null if no such user exists.</returns>
        public Task<User?> GetUserByUsernameAsync(string username);
        /// <summary>
        /// Asynchronously retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
        /// specified email address, or null if no user is found.</returns>
        public Task<User?> GetUserByEmailAsync(string email);
        /// <summary>
        /// Asynchronously adds a new user to the system.
        /// </summary>
        /// <param name="user">The user to add. Cannot be null. The user's properties must meet any required validation constraints.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public Task AddUserAsync(User user);
        /// <summary>
        /// Asynchronously deletes the user with the specified identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains a boolean indicating whether the user was successfully deleted.</returns>
        public Task<bool> DeleteUserAsync(int userId);
    }
}
