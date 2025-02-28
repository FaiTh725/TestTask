using Event.Dal.Repositories;
using Event.Domain.Common;
using Event.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Event.Dal.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private IDbContextTransaction transaction;

        private readonly EventmemberRepository eventMemberRepository;
        private readonly EventRepository eventRepository;

        public UnitOfWork(
            AppDbContext context,
            IEventMemberRepository eventMemberRepository,
            IEventRepository eventRepository)
        {
            this.context = context;

            this.eventMemberRepository = (EventmemberRepository)eventMemberRepository;
            this.eventRepository = (EventRepository)eventRepository;
        }

        public IEventMemberRepository EventMemberRepository => eventMemberRepository;

        public IEventRepository EventRepository => eventRepository;

        public void BeginTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }

        public bool CanConnect()
        {
            return context.Database.CanConnect();
        }

        public async Task<bool> CanConnectAsync()
        {
            return await context.Database.CanConnectAsync();
        }

        public void CommitTransaction()
        {
            EnsureTransaction();

            transaction.Commit();
        }

        public async Task CommitTransactionAsync()
        {
            EnsureTransaction();

            await transaction.CommitAsync();
        }

        public void Dispose()
        {
            transaction?.Dispose();

            context.Dispose();
        }

        public void RollBack()
        {
            EnsureTransaction();

            transaction.Rollback();
        }

        public async Task RollBackAsync()
        {
            EnsureTransaction();

            await transaction.RollbackAsync();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        private void EnsureTransaction()
        {
            if (transaction is null)
            {
                throw new InvalidOperationException("Transaction is null");
            }
        }
    }
}
