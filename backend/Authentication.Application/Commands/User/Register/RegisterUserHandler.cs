using Application.Shared.Exceptions;
using Authentication.Domain.Common;
using UserEntity = Authentication.Domain.Entities.User;
using MediatR;
using Authentication.Application.Interfaces;

namespace Authentication.Application.Commands.User.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHashService hashService;

        public RegisterUserHandler(
            IUnitOfWork unitOfWork, 
            IHashService hashService)
        {
            this.unitOfWork = unitOfWork;
            this.hashService = hashService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository
                .GetUser(request.Email, cancellationToken);

            if(user is not null)
            {
                throw new ConflictApiException("Current Email Already Registered");
            }

            var isCorrectPassword = UserEntity.CheckPasswordForValid(request.Password);

            if (!isCorrectPassword)
            {
                throw new BadRequestApiException("Password Should Has One Letter And One Number");
            }

            var userRole = await unitOfWork.RoleRepository
                .GetRole("User", cancellationToken) ??
                throw new InternalServerApiException("Unknown Server Error");

            var passwordHash = hashService.GenerateHash(request.Password);

            var initUser = UserEntity.Initialize(
                request.Email,
                passwordHash,
                userRole);

            if (initUser.IsFailure)
            {
                throw new BadRequestApiException(initUser.Error);
            }

            var newUser = await unitOfWork.UserRepository
                .AddUser(initUser.Value, cancellationToken);

            await unitOfWork.SaveChangesAsync();

            return newUser.Email;
        }
    }
}
