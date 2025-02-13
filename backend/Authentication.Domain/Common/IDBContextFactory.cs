namespace Authentication.Domain.Common
{
    public interface IDBContextFactory
    {
        Task<bool> CanConnection(CancellationToken cancellationToken);
    }
}
