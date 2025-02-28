using Event.Application.Command.Event.UpdateEvent;
using FluentValidation;

namespace Event.API.Validators.Event
{
    public class UpdateEventValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventValidator()
        {
            RuleFor(x => x.Location)
                .NotNull()
                .NotEmpty()
                .WithMessage("Location Is Requred");

            RuleFor(x => x.TimeEvent)
                .NotNull()
                .WithMessage("Time Event Is Required");

            RuleFor(x => x.MaxMember)
                .NotNull()
                .Must(x => x > 0)
                .WithMessage("Max Members Is Requred And Should Ne Greate Than Zero");
        }
    }
}
