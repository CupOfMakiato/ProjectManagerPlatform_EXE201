using FluentValidation;
using Microsoft.AspNetCore.Http;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Validations.BoardValidations
{
    public class UpdateBoardRequestValidator : AbstractValidator<UpdateBoardRequest>
    {
        public UpdateBoardRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            //RuleFor(x => x.ThumbNail)
            //    .Must(BeAValidImage).WithMessage("File must be a valid image (jpg, jpeg, png) and less than or equal to 2MB.");
        }
        // for later
        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return false;

            if (file.Length > 2 * 1024 * 1024)
                return false;

            return true;
        }
    }
}
