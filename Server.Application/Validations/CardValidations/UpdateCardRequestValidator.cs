using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using Server.Contracts.DTO.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class UpdateCardRequestValidator : AbstractValidator<UpdateCardRequest>
    {
        public UpdateCardRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");

            RuleFor(x => x.AssignedCompletion).NotEmpty().WithMessage("AssignedCompletion is required");
        }
    }
}
