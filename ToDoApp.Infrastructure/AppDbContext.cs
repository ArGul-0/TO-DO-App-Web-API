using ToDoApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure
{
    internal class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<User> Users => Set<User>();

        IQueryable<User> IAppDbContext.Users => Users;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // Constructor that takes DbContextOptions and passes it to the base DbContext constructor
        {

        }

        // Override the OnModelCreating method to configure the model using the ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
