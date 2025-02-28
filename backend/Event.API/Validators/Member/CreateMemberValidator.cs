using Event.Application.Command.EventMember.PaticipateMember;
using FluentValidation;

namespace Event.API.Validators.Member
{
    public class CreateMemberValidator : AbstractValidator<PaticipateMemberCommand>
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
