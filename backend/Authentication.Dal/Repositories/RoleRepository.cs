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

        public async Task<Role> AddRole(Role role)
        {
            var resultEntity = await context.AddAsync(role);

            return resultEntity.Entity;
        }

        public async Task DeleteRole(string roleName)
        {
            await context.Roles
                .Where(x => x.RoleName == roleName)
                .ExecuteDeleteAsync();
        }

        public async Task<Role?> GetRole(string roleName)
        {
            return await context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == roleName);
        }

        public IEnumerable<Role> GetRoles()
        {
            return context.Roles.AsEnumerable();
        }
    }
}
