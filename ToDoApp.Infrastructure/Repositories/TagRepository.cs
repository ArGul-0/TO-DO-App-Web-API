using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Repositories
{
    internal class TagRepository : ITagRepository
    {
        public Task<Tag?> GetTagByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetTagByIdWithTrackingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tag>> GetAllTagsWithOwnersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetTagWithOwnerByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tag>> GetAllTagsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddTagAsync(Tag tag)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTagAsync(int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
