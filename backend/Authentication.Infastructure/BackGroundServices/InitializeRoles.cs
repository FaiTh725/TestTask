using Application.Shared.Exceptions;
using Authentication.Domain.Common;
using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication.Infastructure.BackGroundServices
{
    public class InitializeRoles : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public InitializeRoles(
            IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await WaitDatabase(stoppingToken);

            using var scope = scopeFactory.CreateAsyncScope();

            var dbTransaction = scope.ServiceProvider.GetRequiredService<IDBTransaction>();

            var roles = new string[]
            {
                "User",
                "Admin"
            };

           
            await dbTransaction.StartTransaction();

            var addRolesTasks = roles.Select(async x  =>
            {
                using var innerScope = scopeFactory.CreateAsyncScope();
                var roleRepository = innerScope.ServiceProvider.GetRequiredService<IRoleRepository>();

                var roleDB = await roleRepository.GetRole(x);

                if (roleDB.IsSuccess)
                {
                    return;
                }

                var role = Role.Initialize(x);

                if (role.IsFailure)
                {
                    await dbTransaction.RollBack();
                    throw new AplicationConfigurationException("", "Initialize Roles");
                }

                await roleRepository.AddRole(role.Value);
            }).ToList();

            await Task.WhenAll(addRolesTasks);
            await dbTransaction.Commit();
        }

        private async Task WaitDatabase(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDBContextFactory>();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (await dbContextFactory.CanConnection(cancellationToken))
                {
                    return;
                }

                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
