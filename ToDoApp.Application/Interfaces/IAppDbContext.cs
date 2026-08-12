using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces
{
    public interface IAppDbContext
    {
        IQueryable<User> Users  { get; }
        IQueryable<Note> Notes { get; }
        IQueryable<Friendship> Friendships { get; }
        IQueryable<Tag> Tags { get; }
        IQueryable<NoteTag> NoteTags { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
