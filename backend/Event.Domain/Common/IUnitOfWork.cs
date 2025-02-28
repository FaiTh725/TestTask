using Event.Domain.Repositories;

namespace Event.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        public IEventMemberRepository EventMemberRepository { get; }

        public IEventRepository EventRepository { get; }

        public int SaveChanges();

        public Task<int> SaveChangesAsync();

        public void CommitTransaction();

        public Task CommitTransactionAsync();

        public void RollBack();

        public Task RollBackAsync();

        public void BeginTransaction();

        public Task BeginTransactionAsync();

        public bool CanConnect();

        public Task<bool> CanConnectAsync();
    }
}
