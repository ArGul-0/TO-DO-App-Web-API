using Microsoft.EntityFrameworkCore;
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
            if(userId < 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            if (username is null)
                throw new ArgumentNullException(nameof(username));
                
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (email is null)
                throw new ArgumentNullException(nameof(email));

            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task AddUserAsync(User user)
        {
            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
