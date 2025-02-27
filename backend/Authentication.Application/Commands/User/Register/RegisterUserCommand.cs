using MediatR;

namespace Authentication.Application.Commands.User.Register
{
    public class RegisterUserCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
