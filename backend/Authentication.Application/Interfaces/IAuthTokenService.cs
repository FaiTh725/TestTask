using Authentication.Application.Model.Token;
using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Application.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateToken(User user);

        Result<TokenResponse> DecodeToken(string token);
    }
}
