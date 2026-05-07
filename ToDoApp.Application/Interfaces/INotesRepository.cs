using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces
{
    public interface INotesRepository
    {
        /// <summary>
        /// Asynchronously retrieves all notes associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose notes are to be retrieved. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of notes belonging to the
        /// specified user. The list will be empty if the user has no notes.</returns>
        public Task<List<Note>> GetAllNotesAsync(int userId);
    }
}
