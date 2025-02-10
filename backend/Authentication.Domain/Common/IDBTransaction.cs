namespace Authentication.Domain.Common
{
    public interface IDBTransaction : IDisposable
    {
        Task Commit();

        Task StartTransaction();

        Task RollBack();
    }
}
