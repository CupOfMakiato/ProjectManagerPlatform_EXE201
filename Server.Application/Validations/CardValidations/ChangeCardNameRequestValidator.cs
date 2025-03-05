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
    public class ChangeCardNameRequestValidator : AbstractValidator<ChangeCardNameRequest>
    {
        public ChangeCardNameRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required!");
        }
    }
}
