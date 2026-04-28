using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces
{
    public interface IAppDbContext
    {
        IQueryable<User> Users  { get; }
        IQueryable<Note> Notes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
