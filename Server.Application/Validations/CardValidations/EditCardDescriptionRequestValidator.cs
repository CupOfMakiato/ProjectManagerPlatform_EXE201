using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class EditCardDescriptionRequestValidator : AbstractValidator<EditCardDescriptionRequest>
    {
        public EditCardDescriptionRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required!");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required!");
        }
    }
}
