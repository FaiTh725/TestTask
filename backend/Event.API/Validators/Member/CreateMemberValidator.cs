using Event.API.Contracts.Member;
using FluentValidation;

namespace Event.API.Validators.Member
{
    public class CreateMemberValidator : AbstractValidator<CreateMemberRequest>
    {
        public CreateMemberValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress()
                .WithMessage("Email Is Invalid Signature");

            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage("First Name Is Required");

            RuleFor(x => x.SecondName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Second Name Is Required");
        }
    }
}
