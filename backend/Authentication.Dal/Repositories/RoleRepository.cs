using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Dal.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext context;

        public RoleRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Role> AddRole(
            Role role, 
            CancellationToken token = default)
        {
            var resultEntity = await context.AddAsync(role, token);

            return resultEntity.Entity;
        }

        public async Task DeleteRole(
            string roleName, 
            CancellationToken token = default)
        {
            await context.Roles
                .Where(x => x.RoleName == roleName)
                .ExecuteDeleteAsync(token);
        }

        public async Task<Role?> GetRole(
            string roleName, 
            CancellationToken token = default)
        {
            return await context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == roleName, token);
        }

        public IEnumerable<Role> GetRoles()
        {
            return context.Roles.AsEnumerable();
        }
    }
}
