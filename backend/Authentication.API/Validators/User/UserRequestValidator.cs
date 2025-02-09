using Authentication.Application.Model.User;
using FluentValidation;
using UserEntity = Authentication.Domain.Entities.User;
    
namespace Authentication.API.Validators.User
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress()
                .WithMessage("Email Has Invlid Signature");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(UserEntity.PASSWORD_MIN_LENGTH)
                .WithMessage("Min Password Lenght Is " +
                    UserEntity.PASSWORD_MIN_LENGTH.ToString())
                .MaximumLength(UserEntity.PASSWORD_MAX_LENGTH)
                .WithMessage("Max Password Lenght Is " + 
                    UserEntity.PASSWORD_MAX_LENGTH.ToString());
        }
    }
}
