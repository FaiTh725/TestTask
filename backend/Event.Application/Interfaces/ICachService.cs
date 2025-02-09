using CSharpFunctionalExtensions;

namespace Event.Application.Interfaces
{
    public interface ICachService
    {
        Task<Result<T>> GetData<T>(string key);

        Task<IEnumerable<T>> GetDataFolder<T>(string folderPattern);

        Task SetData<T>(string key, T value, int durationSeconds = 120);

        Task RemoveData(string key);
    }
}
