using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class MoveCardInColumnRequestValidator : AbstractValidator<MoveCardInColumnRequest>
    {
        public MoveCardInColumnRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("CardId is required");

            RuleFor(x => x.ColumnId).NotEmpty().WithMessage("ColumnId is required");

            RuleFor(x => x.NewPosition).NotEmpty().WithMessage("Position is required");
        }
        
    }
}
