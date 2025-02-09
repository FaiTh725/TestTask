using Authentication.Application.Model.Token;

namespace Authentication.Application.Model.User
{
    public class UserResponse
    {
        public string Token { get; set; } = string.Empty;

        public TokenResponse? TokenResponse { get; set; }
    }
}
