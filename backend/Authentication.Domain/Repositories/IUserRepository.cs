using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user, CancellationToken token = default);

        Task<User?> GetUser(string userEmail, CancellationToken token = default);

        Task<User?> GetUser(long userId, CancellationToken token = default);
    }
}
