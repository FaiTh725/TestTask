using Authentication.Application.Model.User;
using MediatR;

namespace Authentication.Application.Queries.User.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<UserResponse>
    {
        public string Email { get; set; } = string.Empty;
    }
}
