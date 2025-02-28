using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> AddUser(
            User user, 
            CancellationToken token = default)
        {
            var userEntity = await context.AddAsync(user, token);

            return userEntity.Entity;
        }

        public async Task<User?> GetUser(
            string userEmail, 
            CancellationToken token = default)
        {
            return await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == userEmail, token);
        }

        public async Task<User?> GetUser(
            long userId, 
            CancellationToken token = default)
        {
            return await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId, token);
        }
    }
}
