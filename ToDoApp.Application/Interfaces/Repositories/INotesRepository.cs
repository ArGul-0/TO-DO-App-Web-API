using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface INotesRepository
    {
        /// <summary>
        /// Asynchronously retrieves all notes associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose notes are to be retrieved. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes belonging to the
        /// specified user. The list will be empty if the user has no notes.</returns>
        public Task<List<Note>> GetAllMyNotesAsync(int userId);

        /// <summary>
        /// Asynchronously retrieves a note by its unique identifier and the associated user's identifier. This method is used to fetch a specific note that belongs to a user, ensuring that the note is only accessible if it is associated with the provided user ID. If the note exists and belongs to the user, it will be returned; otherwise, null will be returned, indicating that either the note does not exist or it does not belong to the specified user.
        /// </summary>
        /// <param name="id">The unique identifier of the note to be retrieved. Must be a positive integer.</param>
        /// <param name="userId">The unique identifier of the user who owns the note. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the note if it exists and belongs to the specified user; otherwise, null.</returns>
        public Task<Note?> GetNoteByIdAsync(int id, int userId);
    }
}
