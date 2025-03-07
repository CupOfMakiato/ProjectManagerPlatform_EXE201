using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class MoveCardToColumnRequestValidator : AbstractValidator<MoveCardToColumnRequest>
    {
        public MoveCardToColumnRequestValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Card ID is required.");

            RuleFor(x => x.NewColumnId)
                .NotEmpty().WithMessage("Target Column ID is required.");

            RuleFor(x => x.NewPosition)
                .GreaterThan(0).WithMessage("New position must be at least 1.");
        }
    }
}
