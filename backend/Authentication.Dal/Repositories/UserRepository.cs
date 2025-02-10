using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using CSharpFunctionalExtensions;
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

        public async Task<Result<User>> AddUser(User user)
        {
            if(user is null)
            {
                return Result.Failure<User>("User Is Null");
            }

            var userEntity = await context.AddAsync(user);

            await context.SaveChangesAsync();

            return Result.Success<User>(userEntity.Entity);
        }

        public async Task<Result<User>> GetUser(string userEmail)
        {
            var user = await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == userEmail);
        
            if(user is null)
            {
                return Result.Failure<User>("User Not Found");
            }

            return Result.Success(user);
        }

        public async Task<Result<User>> GetUser(long userId)
        {
            var user = await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return Result.Failure<User>("User Not Found");
            }

            return Result.Success(user);
        }
    }
}
