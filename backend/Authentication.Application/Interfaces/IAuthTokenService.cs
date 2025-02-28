using Authentication.Application.Model.User;

namespace Authentication.Application.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateToken(UserResponse user);
    }
}
