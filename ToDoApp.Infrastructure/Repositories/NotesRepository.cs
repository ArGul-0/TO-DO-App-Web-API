using ToDoApp.Domain.Entities;
using ToDoApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Infrastructure.Repositories
{
    internal class NotesRepository : INotesRepository
    {
        private readonly AppDbContext dbContext;

        public NotesRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Note>> GetAllNotesAsync(int userId)
        {
            return await dbContext.Notes
                .AsNoTracking()
                .Where(note => note.UserId == userId)
                .ToListAsync();
        }
    }
}
