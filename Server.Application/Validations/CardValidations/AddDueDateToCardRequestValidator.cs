using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class AddDueDateToCardRequestValidator : AbstractValidator<AddDueDateToCardRequest>
    {
        public AddDueDateToCardRequestValidator()
        {
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("Card ID is required.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.DueDate)
                .When(x => x.StartDate.HasValue)
                .WithMessage("Start date must be before the due date.");

            RuleFor(x => x.Reminder)
            .IsInEnum().WithMessage("Invalid reminder option.");
        }
    }
}
