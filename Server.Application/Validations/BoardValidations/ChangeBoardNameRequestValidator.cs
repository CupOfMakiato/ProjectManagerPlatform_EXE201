using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.BoardValidations
{
    public class ChangeBoardNameRequestValidator : AbstractValidator<ChangeBoardNameRequest>
    {
        public ChangeBoardNameRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

         }
    }
}
