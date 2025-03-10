using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.AttachmentValidations
{
    public class ChangeAttachmentNameRequestValidator : AbstractValidator<ChangeAttachmentNameRequest>
    {
        public ChangeAttachmentNameRequestValidator()
        {
            RuleFor(x => x.attachmentId)
                .NotNull().WithMessage("attachmentId is required.")
                .NotEqual(Guid.Empty).WithMessage("attachmentId is required.");

            RuleFor(x => x.FileName)
                .NotNull().WithMessage("File name is required.")
                .NotEmpty().WithMessage("File name is required.")
                .MaximumLength(255).WithMessage("File name must be less than 100 characters.");
        }
    }
}
