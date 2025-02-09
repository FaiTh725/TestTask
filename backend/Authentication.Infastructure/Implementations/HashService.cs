using Authentication.Application.Interfaces;

namespace Authentication.Infastructure.Implementations
{
    public class HashService : IHashService
    {
        public string GenerateHash(string inputValue)
        {
            return BCrypt.Net.BCrypt.HashPassword(inputValue);
        }

        public bool VerifyHash(string inputValue, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(inputValue, hash);
        }
    }
}
