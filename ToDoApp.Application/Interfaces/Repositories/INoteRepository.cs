using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces.Repositories
{
    public interface INoteRepository
    {
        /// <summary>
        /// Asynchronously retrieves a note by id without tracking.
        /// The returned Note includes its NoteTags collection and each NoteTag's Tag entity.
        /// </summary>
        /// <param name="id">Note id.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the note with its tags if found; otherwise, null.
        /// </returns>
        public Task<Note?> GetNoteByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves a note by ID with tracking.
        /// The returned Note includes its NoteTags collection and each NoteTag's Tag entity.
        /// </summary>
        /// <param name="id">Note ID.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the note with its tags if found; otherwise, null.
        /// </returns>
        public Task<Note?> GetNoteByIdWithTrackingAsync(int id);

        /// <summary>
        /// Asynchronously retrieves a note by ID with tracking and includes its associated NoteTags
        /// and each NoteTag's Tag entity.
        /// </summary>
        /// <param name="id">Note ID.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the note with its tags if found; otherwise, null.
        /// </returns>
        public Task<Note?> GetNoteByIdWithTrackingIncludeNoteTagsAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all notes with their owners.
        /// Each returned Note includes its NoteTags collection and each NoteTag's Tag entity.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of notes with their owners and tags.
        /// The list will be empty if no notes are found.
        /// </returns>
        public Task<List<Note>> GetAllNotesWithOwnersAsync();

        /// <summary>
        /// Asynchronously retrieves a note by its unique identifier with its owner.
        /// The returned Note includes its NoteTags collection and each NoteTag's Tag entity.
        /// </summary>
        /// <param name="id">The unique identifier of the note.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the note with its owner and tags if found; otherwise, null.
        /// </returns>
        public Task<Note?> GetNoteWithOwnerByIdAsync(int id);

        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a list of the user's notes; each Note includes its NoteTags
        /// and each NoteTag's Tag entity. The list will be empty if the user has no notes.
        /// </returns>
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
        /// <param name="note">The note to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public Task DeleteNoteAsync(Note note);
    }
}
