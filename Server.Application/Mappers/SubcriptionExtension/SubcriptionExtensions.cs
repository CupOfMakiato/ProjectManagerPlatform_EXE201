using Server.Application.Mappers.UserExtension;
using Server.Contracts.Abstractions.RequestAndResponse.Board;
using Server.Contracts.Abstractions.RequestAndResponse.Subcription;
using Server.Contracts.DTO.Board;
using Server.Contracts.DTO.Subcription;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Mappers.SubcriptionExtension
{
    public static class SubcriptionExtensions
    {
        public static ViewSubcriptionDTO ToViewSubcriptionDTO(this Subcription Subcription)
        {
            return new ViewSubcriptionDTO
            {
                Id = Subcription.Id,
                SubcriptionName = Subcription.SubcriptionName,
                Description = Subcription.Description,
                Price = Subcription.Price,
                Duration = Subcription.Duration,
                CreatedByUser = Subcription.SubcriptionCreatedBy.ToUserDTO()

            };
        }

        public static Subcription ToSubcription(this AddNewSubcriptionDTO AddNewSubcriptionDTO)
        {
            return new Subcription
            {
                Id = AddNewSubcriptionDTO.Id,
                SubcriptionName = AddNewSubcriptionDTO.SubcriptionName,
                Description = AddNewSubcriptionDTO.Description,
                Price = AddNewSubcriptionDTO.Price,
                Duration = AddNewSubcriptionDTO.Duration,
                CreatedBy = AddNewSubcriptionDTO.UserId,

            };
        }
        public static AddNewSubcriptionDTO ToAddNewSubcriptionDTO(this AddNewSubcriptionRequest AddNewSubcriptionRequest)
        {
            return new AddNewSubcriptionDTO
            {
                Id = (Guid)AddNewSubcriptionRequest.Id,
                UserId = AddNewSubcriptionRequest.UserId,
                SubcriptionName = AddNewSubcriptionRequest.SubcriptionName,
                Description = AddNewSubcriptionRequest.Description,
                Price = AddNewSubcriptionRequest.Price,
                Duration = 30,

            };
        }
        public static UpdateSubcriptionDTO ToUpdateSubcriptionDTO(this UpdateSubcriptionRequest UpdateSubcriptionRequest)
        {
            return new UpdateSubcriptionDTO
            {
                Id = (Guid)UpdateSubcriptionRequest.Id,
                SubcriptionName = UpdateSubcriptionRequest.SubcriptionName,
                Description = UpdateSubcriptionRequest.Description,
                Price = UpdateSubcriptionRequest.Price,
            };
        }
    }
}
