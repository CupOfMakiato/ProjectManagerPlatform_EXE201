using FluentValidation;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.CardValidations
{
    public class DownloadAttachmentRequestValidator : AbstractValidator<DownloadAttachmentRequest>
    {
        public DownloadAttachmentRequestValidator()
        {
            
        }
    }
}
