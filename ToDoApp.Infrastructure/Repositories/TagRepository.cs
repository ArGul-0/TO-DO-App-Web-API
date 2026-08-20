using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Repositories
{
    internal class TagRepository : ITagRepository
    {
        private readonly AppDbContext dbContext;
        public TagRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await dbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag?> GetTagByIdWithTrackingAsync(int id)
        {
            return await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tag>> GetAllTagsWithOwnersAsync()
        {
            return await dbContext.Tags
                .AsNoTracking()
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<Tag?> GetTagWithOwnerByIdAsync(int id)
        {
            return await dbContext.Tags
                .AsNoTracking()
                .Include(t => t.Owner)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tag>> GetAllTagsByUserIdAsync(int userId)
        {
            return await dbContext.Tags
                .AsNoTracking()
                .Where(t => t.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddTagAsync(Tag tag)
        {
            await dbContext.Tags.AddAsync(tag);

            return true;
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            var tag = await dbContext.Tags.FindAsync(tagId);

            if (tag is null)
                return false;

            dbContext.Tags.Remove(tag);

            return true;
        }
    }
}
