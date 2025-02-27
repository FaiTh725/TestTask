using Authentication.Domain.Repositories;

namespace Authentication.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }

        public IRoleRepository RoleRepository { get; }

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
