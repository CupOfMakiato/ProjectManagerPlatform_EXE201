using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Subcription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.SubcriptionValidations
{
    public class AddNewSubcriptionRequestValidator : AbstractValidator<AddNewSubcriptionRequest>
    {
        public AddNewSubcriptionRequestValidator()
        {
            RuleFor(x => x.SubcriptionName)
                .NotEmpty()
                .WithMessage("Subcription name is required.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Duration)
                .GreaterThan(0)
                .WithMessage("Duration must be greater than 0.");
        }
    }
}
