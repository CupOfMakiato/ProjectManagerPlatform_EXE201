using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Subcription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.SubcriptionValidations
{
    public class UpdateSubcriptionRequestValidator : AbstractValidator<UpdateSubcriptionRequest>
    {
        public UpdateSubcriptionRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
            RuleFor(x => x.SubcriptionName)
                .NotEmpty()
                .WithMessage("SubcriptionName is required.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be greater than 0.");
        }
    }
}
