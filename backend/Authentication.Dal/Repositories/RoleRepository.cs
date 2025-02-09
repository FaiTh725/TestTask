using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using CSharpFunctionalExtensions;
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

        public async Task<Result<Role>> AddRole(Role role)
        {
            if (role is null)
            {
                return Result.Failure<Role>("Role Is Null");
            }

            var resultEntity = await context.AddAsync(role);

            await context.SaveChangesAsync();

            return Result.Success(resultEntity.Entity);
        }

        public async Task DeleteRole(string roleName)
        {
            await context.Roles
                .Where(x => x.RoleName == roleName)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
        }

        public async Task<Result<Role>> GetRole(string roleName)
        {
            var role = await context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == roleName);

            if(role is null)
            {
                return Result.Failure<Role>("Role Not Found");
            }

            return Result.Success(role);
        }

        public IQueryable<Role> GetRoles()
        {
            return context.Roles;
        }
    }
}
