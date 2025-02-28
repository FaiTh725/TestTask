using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddRole(Role role, CancellationToken token = default);

        IEnumerable<Role> GetRoles();

        Task DeleteRole(string roleName, CancellationToken token = default);

        Task<Role?> GetRole(string roleName, CancellationToken token = default);
    }
}
