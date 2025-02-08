using CSharpFunctionalExtensions;
using Event.Application.Interfaces;

namespace Event.Infastructure.Implementations
{
    public class CashService : ICachService
    {
        public Task<Result<T>> GetData<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveData(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetData<T>(string key, T value, int durationSeconds = 120)
        {
            throw new NotImplementedException();
        }
    }
}
