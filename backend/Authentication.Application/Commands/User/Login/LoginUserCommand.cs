using MediatR;

namespace Authentication.Application.Commands.User.Login
{
    public class LoginUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
