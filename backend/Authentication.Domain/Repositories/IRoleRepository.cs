using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddRole(Role role);

        IEnumerable<Role> GetRoles();

        Task DeleteRole(string roleName);

        Task<Role?> GetRole(string roleName);
    }
}
