using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);

        Task<User?> GetUser(string userEmail);

        Task<User?> GetUser(long userId);
    }
}
