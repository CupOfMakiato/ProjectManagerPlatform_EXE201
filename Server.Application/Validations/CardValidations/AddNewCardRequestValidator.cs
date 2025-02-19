using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class AddNewCardRequestValidator : AbstractValidator<AddNewCardRequest>
    {
        public AddNewCardRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required");

            RuleFor(x => x.ColumnId)
                .NotEmpty().WithMessage("ColumnId is required");
            
        }
    }
}
