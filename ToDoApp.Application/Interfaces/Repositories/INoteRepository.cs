using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface INoteRepository
    {
        /// <summary>
        /// Asynchronously retrieves a note by id without tracking.
        /// </summary>
        /// <param name="id">Note id.</param>
        /// <returns>The note if found; otherwise null.</returns>
        public Task<Note?> GetNoteById(int id);

        /// <summary>
        /// Asynchronously retrieves a note by id with tracking.
        /// </summary>
        /// <param name="id">Note id.</param>
        /// <returns>The tracked note if found; otherwise null.</returns>
        public Task<Note?> GetNoteByIdWithTracking(int id);

        /// <summary>
        /// Asynchronously retrieves all notes.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes. The list will be empty.</returns>
        public Task<List<Note>> GetAllNotesWithOwnersAsync();

        /// <summary>
        /// Asynchronously retrieves a note by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the note to be retrieved. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the note if it exists.</returns>
        public Task<Note?> GetNoteWithOwnerByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all notes for a user.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>List of notes (may be empty).</returns>
        public Task<List<Note>> GetAllNotesByUserIdAsync(int userId);

        /// <summary>
        /// Asynchronously adds a new note to the repository.
        /// </summary>
        /// <param name="note">The note to be added.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the operation was successful.</returns>
        public Task<bool> AddNoteAsync(Note note);

        /// <summary>
        /// Asynchronously deletes a note from the repository.
        /// </summary>
        /// <param name="noteId">The id of the note to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the operation was successful.</returns>
        public Task<bool> DeleteNoteAsync(int noteId);
    }
}
