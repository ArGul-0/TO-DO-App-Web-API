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

        public async Task<List<Note>> GetAllNotesAsync()
        {
            return await dbContext.Notes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(note => note.Id == id);
        }

        public async Task<List<Note>> GetAllNotesByUserIdAsync(int userId)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .Where(note => note.UserId == userId)
                .ToListAsync();
        }
    }
}
