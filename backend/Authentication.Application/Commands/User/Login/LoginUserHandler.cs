using Application.Shared.Exceptions;
using Authentication.Application.Interfaces;
using Authentication.Domain.Common;
using MediatR;

namespace Authentication.Application.Commands.User.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHashService hashService;

        public LoginUserHandler(
            IUnitOfWork unitOfWork, 
            IHashService hashService)
        {
            this.unitOfWork = unitOfWork;
            this.hashService = hashService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository
                .GetUser(request.Email, cancellationToken) ?? 
                throw new NotFoundApiException("Current Email Does Not Exist");

            if (!hashService.VerifyHash(request.Password, user.Password))
            {
                throw new BadRequestApiException("Incorrect Password Or Email");
            }

            await unitOfWork.SaveChangesAsync();

            return user.Email;
        }
    }
}
