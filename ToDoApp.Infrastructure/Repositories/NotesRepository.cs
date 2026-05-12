using ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Infrastructure.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly AppDbContext dbContext;

        public NotesRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Note>> GetAllNotesWithOwnersAsync()
        {
            return await dbContext.Notes
                .AsNoTracking()
                .Include(note => note.User)
                .ToListAsync();
        }

        public async Task<Note?> GetNoteWithOwnerByIdAsync(int id)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .Include(note => note.User)
                .FirstOrDefaultAsync(note => note.Id == id);
        }

        public async Task<List<Note>> GetAllNotesByUserIdAsync(int userId)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .Where(note => note.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            var note = await dbContext.Notes.FindAsync(noteId);

            if (note is null)
                return false;

            dbContext.Notes.Remove(note);

            return true;
        }
    }
}
