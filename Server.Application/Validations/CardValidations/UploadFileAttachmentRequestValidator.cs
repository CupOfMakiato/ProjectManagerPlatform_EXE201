using FluentValidation;
using Microsoft.AspNetCore.Http;
using Server.Contracts.Abstractions.RequestAndResponse.Card;
using System.IO;
using System.Linq;

namespace Server.Application.Validations.CardValidate
{
    public class UploadFileAttachmentRequestValidator : AbstractValidator<UploadFileAttachmentRequest>
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = 
            { ".jpg", ".jpeg", ".png", ".gif", ".pdf",
            ".docx", ".doc", ".pptx", ".ppt", ".pdf",
            ".xlsx", ".ods", ".pptx", ".txt", ".rar", ".zip", ".7z",
            ".ogg", ".wav", ".mp3"};

        public UploadFileAttachmentRequestValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(BeAValidFile).WithMessage($"File must be one of the allowed types: {string.Join(", ", _allowedExtensions)} and less than 5MB.");
        }

        private bool BeAValidFile(IFormFile file)
        {
            if (file == null)
                return false;

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return false;

            if (file.Length > _maxFileSize)
                return false;

            return true;
        }
    }
}
