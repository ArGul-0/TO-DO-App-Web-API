using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        /// <summary>
        /// Asynchronously retrieves a tag by its unique identifier without tracking.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the tag if found; otherwise, null.
        /// </returns>
        public Task<Tag?> GetTagByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves a tag by its unique identifier with tracking enabled.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the tracked tag if found; otherwise, null.
        /// </returns>
        public Task<Tag?> GetTagByIdWithTrackingAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all tags with their owners.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of tags with their owners.
        /// The list will be empty if no tags are found.
        /// </returns>
        public Task<List<Tag>> GetAllTagsWithOwnersAsync();

        /// <summary>
        /// Asynchronously retrieves a tag by its unique identifier with its owner.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the tag with its owner if found; otherwise, null.
        /// </returns>
        public Task<Tag?> GetTagWithOwnerByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all tags belonging to a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of the user's tags.
        /// The list will be empty if the user has no tags.
        /// </returns>
        public Task<List<Tag>> GetAllTagsByUserIdAsync(int userId);

        /// <summary>
        /// Asynchronously adds a new tag to the repository.
        /// </summary>
        /// <param name="tag">The tag to be added.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result indicates whether the operation was successful.
        /// </returns>
        public Task<bool> AddTagAsync(Tag tag);

        /// <summary>
        /// Asynchronously deletes a tag from the repository.
        /// </summary>
        /// <param name="tag">The tag to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// </returns>
        public Task DeleteTagAsync(Tag tag);
    }
}