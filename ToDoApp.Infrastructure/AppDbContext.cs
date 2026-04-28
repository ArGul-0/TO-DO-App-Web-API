using ToDoApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Note> Notes => Set<Note>();

        IQueryable<User> IAppDbContext.Users => Users;
        IQueryable<Note> IAppDbContext.Notes => Notes;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // Constructor that takes DbContextOptions and passes it to the base DbContext constructor
        {

        }

        // Override the OnModelCreating method to configure the model using the ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the User entity to own the Email value object
            modelBuilder.Entity<User>(builder =>
            builder.OwnsOne(u => u.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired();
            }));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notes)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
