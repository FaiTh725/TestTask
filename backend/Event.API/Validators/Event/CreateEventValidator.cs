﻿using Event.API.Contracts.Event;
using FluentValidation;

namespace Event.API.Validators.Event
{
    public class CreateEventValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Category)
                .NotNull()
                .NotEmpty()
                .WithMessage("Category Is Requred");

            RuleFor(x => x.Location)
                .NotNull()
                .NotEmpty()
                .WithMessage("Location Is Requred");

            RuleFor(x => x.TimeEvent)
                .NotNull()
                .WithMessage("Time Event Is Required");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name Is Requred");

            RuleFor(x => x.MaxMember)
                .NotNull()
                .Must(x => x > 0)
                .WithMessage("Max Members Is Requred And Should Ne Greate Than Zero");
        }
    }
}
