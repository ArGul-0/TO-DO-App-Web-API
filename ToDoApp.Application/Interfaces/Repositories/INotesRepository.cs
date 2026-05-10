using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface INotesRepository
    {
        /// <summary>
        /// Asynchronously retrieves all notes.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes. The list will be empty.</returns>
        public Task<List<Note>> GetAllNotesAsync();

        /// <summary>
        /// Asynchronously retrieves a note by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the note to be retrieved. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the note if it exists.</returns>
        public Task<Note?> GetNoteByIdAsync(int id);
    }
}
