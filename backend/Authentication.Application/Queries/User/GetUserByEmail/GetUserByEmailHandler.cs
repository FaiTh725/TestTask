using Application.Shared.Exceptions;
using Authentication.Application.Model.User;
using Authentication.Domain.Common;
using MediatR;

namespace Authentication.Application.Queries.User.GetUserByEmail
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetUserByEmailHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository
                .GetUser(request.Email, cancellationToken) ?? 
                throw new NotFoundApiException($"User With Email {request.Email} Doesnt Exist");

            return new UserResponse
            {
                Email = user.Email,
                Role = user.Role.RoleName
            };
        }
    }
}
