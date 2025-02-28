using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Column;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.ColumnValidations
{
    public class AddNewColumsRequestValidator : AbstractValidator<AddNewColumnRequest>
    {
        public AddNewColumsRequestValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.BoardId)
                .NotEmpty().WithMessage("BoardId is required.");
        }
    }
}
