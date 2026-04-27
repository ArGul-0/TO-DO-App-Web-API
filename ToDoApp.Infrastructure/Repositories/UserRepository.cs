using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        { 
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task AddUserAsync(User user)
        {
            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var affected = await dbContext.Users
                .Where(u => u.Id == userId)
                .ExecuteDeleteAsync();

            return affected > 0;
        }
    }
}
