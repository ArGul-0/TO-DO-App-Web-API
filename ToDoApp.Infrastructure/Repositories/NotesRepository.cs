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

        public async Task<List<Note>> GetAllMyNotesAsync(int userId)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .Where(note => note.UserId == userId)
                .ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(int id, int userId)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(note => note.Id == id && note.UserId == userId);
        }
    }
}
