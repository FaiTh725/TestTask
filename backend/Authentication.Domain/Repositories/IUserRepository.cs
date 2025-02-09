using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> AddUser(User user);

        Task<Result<User>> GetUser(string userEmail);

        Task<Result<User>> GetUser(long userId);
    }
}
