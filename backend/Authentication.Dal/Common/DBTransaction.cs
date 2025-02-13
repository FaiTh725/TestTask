
using Authentication.Domain.Common;

namespace Authentication.Dal.Common
{
    public class DBTransaction : IDBTransaction
    {
        private readonly AppDbContext context;

        public DBTransaction(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task Commit()
        {
            await context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task RollBack()
        {
            await context.Database.RollbackTransactionAsync();
        }

        public async Task StartTransaction()
        {
            await context.Database.BeginTransactionAsync();
        }
    }
}
