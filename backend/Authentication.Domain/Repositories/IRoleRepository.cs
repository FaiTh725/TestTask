using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Result<Role>> AddRole(Role role);

        IQueryable<Role> GetRoles();

        Task DeleteRole(string roleName);

        Task<Result<Role>> GetRole(string roleName);
    }
}
