
using Event.Dal;
using Microsoft.EntityFrameworkCore;

namespace Event.API.Infastructure
{
    public class InitializeDBBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<InitializeDBBackgroundService> logger;

        public InitializeDBBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<InitializeDBBackgroundService> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Start Execute DataBase migration");

            using var scope = serviceScopeFactory.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                if(context.Database
                    .GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync(stoppingToken);
                    logger.LogInformation("Successfuly Apply Migration");
                }
            }
            catch
            {
                logger.LogError("Error Apply Migrations");
            }
        }
    }
}
