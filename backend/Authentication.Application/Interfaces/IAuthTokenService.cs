using Authentication.Application.Model.Token;
using Authentication.Application.Model.User;
using Authentication.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authentication.Application.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateToken(User user);

        string GenerateToken(UserResponse user);

        Result<TokenResponse> DecodeToken(string token);
    }
}
