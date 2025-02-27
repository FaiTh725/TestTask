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

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var roles = new string[]
            {
                "User",
                "Admin"
            };

            var existingRoles = unitOfWork.RoleRepository.GetRoles();

            await unitOfWork.BeginTransactionAsync();

            var addRolesTasks = roles.Select(async x  =>
            {
                using var innerScope = scopeFactory.CreateAsyncScope();
                var innerUnitOfWork = innerScope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();

                var roleDB = existingRoles
                    .FirstOrDefault(role => role.RoleName == x);
                
                if (roleDB is not null)
                {
                    return;
                }

                var role = Role.Initialize(x);

                if (role.IsFailure)
                {
                    await innerUnitOfWork.RollBackAsync();
                    throw new AplicationConfigurationException("", "Initialize Roles");
                }

                await innerUnitOfWork.RoleRepository.AddRole(role.Value);
                await innerUnitOfWork.SaveChangesAsync();
            }).ToList();

            await Task.WhenAll(addRolesTasks);
            await unitOfWork.CommitTransactionAsync();
        }

        private async Task WaitDatabase(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider
                .GetRequiredService<IUnitOfWork>();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (await unitOfWork.CanConnectAsync())
                {
                    return;
                }

                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
