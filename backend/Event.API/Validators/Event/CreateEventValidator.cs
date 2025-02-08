using Event.API.Contracts.Event;
using FluentValidation;

namespace Event.API.Validators.Event
{
    public class CreateEventValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventValidator()
        {
            
        }
    }
}
