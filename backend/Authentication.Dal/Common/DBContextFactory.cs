using Authentication.Domain.Common;
namespace Authentication.Dal.Common
{
    public class DBContextFactory : IDBContextFactory
    {
        private readonly AppDbContext context;

        public DBContextFactory(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CanConnection(CancellationToken cancellationToken)
        {
            return await context.Database.CanConnectAsync(cancellationToken);
        }
    }
}
