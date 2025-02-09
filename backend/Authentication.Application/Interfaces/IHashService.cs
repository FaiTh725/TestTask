namespace Authentication.Application.Interfaces
{
    public interface IHashService
    {
        string GenerateHash(string inputValue);

        bool VerifyHash(string inputValue, string hash);
    }
}
